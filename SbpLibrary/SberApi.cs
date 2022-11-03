using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SBPLibrary.Models;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SBPLibrary.Models.Sber;
using SBPLibrary.Models.Sber.Response;

namespace SBPLibrary
{
    /// <summary>
    /// A universal API for Sberbank's SBP.
    /// </summary>
    public interface IPaymentApi
    {
        ErrorModel<RevokeResponse> RevokeOrder(RevokeRequest request);
        ErrorModel<CancelResponse> CancelPayment(CancelRequest request);
        ErrorModel<OrderStatusResponse> GetStatus(OrderStatusRequest request);
        ErrorModel<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request);
    }
    
    /// <summary>
    /// API reference:
    /// https://api.developer.sber.ru/product/PlatiQR/doc/v1/8024874181
    /// </summary>
    public class SberApi : IPaymentApi
    {
        private string _token = "";
        
        private readonly X509Certificate2Collection _certificates;
        private readonly SbpSettings _settings;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _jsonSettings = new()
        {
            DateFormatString = "yyyy-MM-ddTHH:mm:ssZ",
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        private const int HttpTimeoutMs = 25000;

        public SberApi(SbpSettings settings, ILogger logger)
        {
            _logger = logger;
            _settings = settings;
            _certificates = new X509Certificate2Collection();
            _certificates.Import(_settings.CertPath, _settings.CertPassword, 
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
        }

        public ErrorModel<RevokeResponse> RevokeOrder(RevokeRequest request)
        {
            var tokenResponse = GetToken("https://api.sberbank.ru/qr/order.revoke");

            if (tokenResponse.IsError)
            {
                return new ErrorModel<RevokeResponse> { IsError = true, ErrorMsg = tokenResponse.ErrorMsg };
            }

            var url = Path.Combine(_settings.BasePath, "order/v3/revocation");

            request.RqUid = Guid.NewGuid().ToString().Replace("-", "");
            request.RqTm = DateTime.Now;

            var response = ExecuteQuery(request.RqUid, url, JsonConvert.SerializeObject(request, _jsonSettings), "POST");

            try
            {
                var data = JsonConvert.DeserializeObject<RevokeResponse>(response.RespObject);
                
                return response.IsError 
                    ? new ErrorModel<RevokeResponse> { IsError = true, RespObject = data, ErrorMsg = data?.error_description, RespCode = response.RespCode } 
                    : new ErrorModel<RevokeResponse> { IsError = data?.error_description?.Length > 0, RespCode = response.RespCode, 
                        RespObject = data, ErrorMsg = data?.error_description };
            }
            catch (ArgumentNullException e)
            {
                return new ErrorModel<RevokeResponse> { IsError = true, ErrorMsg = $"Bad Request: {e.Message}" };
            }
        }

        public ErrorModel<CancelResponse> CancelPayment(CancelRequest request)
        {
            var tokenResponse = GetToken("https://api.sberbank.ru/qr/order.cancel");

            if (tokenResponse.IsError)
            {
                return new ErrorModel<CancelResponse> { IsError = true, ErrorMsg = tokenResponse.ErrorMsg };
            }
            
            var url = Path.Combine(_settings.BasePath, "order/v3/cancel");

            request.RqUid = Guid.NewGuid().ToString().Replace("-", "");
            request.RqTm = DateTime.Now;

            var response = ExecuteQuery(request.RqUid, url, JsonConvert.SerializeObject(request, _jsonSettings), "POST");
            
            try
            {
                var data = JsonConvert.DeserializeObject<CancelResponse>(response.RespObject);
                
                return response.IsError 
                    ? new ErrorModel<CancelResponse> { IsError = true, RespObject = data, ErrorMsg = data?.error_description, RespCode = response.RespCode } 
                    : new ErrorModel<CancelResponse> { IsError = data?.error_description?.Length > 0, RespCode = response.RespCode, 
                        RespObject = data, ErrorMsg = data?.error_description };
            } catch (ArgumentNullException e)
            {
                return new ErrorModel<CancelResponse> { IsError = true, ErrorMsg = $"Bad Request: {e.Message}" };
            }
        }


        public ErrorModel<OrderStatusResponse> GetStatus(OrderStatusRequest request)
        {
            var tokenResponse = GetToken("https://api.sberbank.ru/qr/order.status");

            if (tokenResponse.IsError)
            {
                return new ErrorModel<OrderStatusResponse> { IsError = true, ErrorMsg = tokenResponse.ErrorMsg };
            }
            
            var url = Path.Combine(_settings.BasePath, "order/v3/status");

            request.RqUid = Guid.NewGuid().ToString().Replace("-", "");
            request.RqTm = DateTime.Now;
            
            var response = ExecuteQuery(request.RqUid, url, JsonConvert.SerializeObject(request, _jsonSettings), "POST");
            try
            {
                var data = JsonConvert.DeserializeObject<OrderStatusResponse>(response.RespObject);
                
                return response.IsError 
                    ? new ErrorModel<OrderStatusResponse> { IsError = true, RespObject = data, ErrorMsg = "Unknown error", RespCode = response.RespCode }
                    : new ErrorModel<OrderStatusResponse> { IsError = false, RespCode = response.RespCode, RespObject = data, ErrorMsg = "" };
            } catch (ArgumentNullException e)
            {
                return new ErrorModel<OrderStatusResponse> { IsError = true, ErrorMsg = $"Bad Request: {e.Message}" };
            }
        }

        public ErrorModel<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request)
        {
            var tokenResponse = GetToken("https://api.sberbank.ru/qr/order.create");

            if (tokenResponse.IsError)
            {
                return new ErrorModel<CreatePaymentResponse> { IsError = true, ErrorMsg = tokenResponse.ErrorMsg };
            }

            var url = Path.Combine(_settings.BasePath, "order/v3/creation");

            request.RqUid = Guid.NewGuid().ToString().Replace("-", "");
            request.RqTm = DateTime.Now;

            var response = ExecuteQuery(request.RqUid, url, JsonConvert.SerializeObject(request, _jsonSettings), "POST");
            
            try
            {
                var data = JsonConvert.DeserializeObject<CreatePaymentResponse>(response.RespObject);
                
                if (response.IsError || data?.order_state != OrderStatus.Created)
                {
                    return new ErrorModel<CreatePaymentResponse> { IsError = true, RespObject = data, ErrorMsg = data?.error_description, 
                        RespCode = response.RespCode };
                }
            
                return new ErrorModel<CreatePaymentResponse> { IsError = data.error_description?.Length > 0, RespCode = response.RespCode, 
                    RespObject = data, ErrorMsg = data.error_description };
            } catch (ArgumentNullException e)
            {
                return new ErrorModel<CreatePaymentResponse> { IsError = true, ErrorMsg = $"Bad Request: {e.Message}" };
            }
            
        }

        private ErrorModel<string> GetToken(string scope)
        {
            try
            {
                var payload = Encoding.UTF8.GetBytes($"grant_type=client_credentials&scope={scope}");

                _logger.Info("GetToken URL: " + _settings.TokenBasePath);
                
                var request = (HttpWebRequest)WebRequest.Create(_settings.TokenBasePath);
                var authString = $"{_settings.ClientId}:{_settings.ClientSecret}";
                var b64AuthString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authString));

                request.ClientCertificates = _certificates;
                request.Headers.Add("Authorization", "Basic " + b64AuthString);
                request.Headers.Add("rquid", Guid.NewGuid().ToString().Replace("-", ""));
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 100000;
                request.Method = "POST";

                _logger.Info("[GetToken] RequestHeaders: " + request.Headers);

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(payload, 0, payload.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8))
                    {
                        var responseRaw = reader.ReadToEnd();
                        var result = new ErrorModel<string> { IsError = false, RespCode = response.StatusCode, RespObject = responseRaw };
                        
                        _logger.Info("[GetToken] Received response: " + JsonConvert.SerializeObject(responseRaw, _jsonSettings));
                        _logger.Info("[GetToken] Object: " + result.RespObject);
                        
                        _token = JsonConvert.DeserializeObject<TokenModel>(result.RespObject)?.access_token;
                        return result;
                    }
                }
            }
            catch (WebException webEx)
            {
                try
                {
                    _logger.Error("Execute webError: " + webEx);

                    using (var reader = new StreamReader(webEx.Response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8))
                    {
                        _logger.Error("Response: " + reader.ReadToEnd());
                    }
                    return new ErrorModel<string> { IsError = true, ErrorMsg = webEx.ToString(), RespCode = ((HttpWebResponse)webEx.Response).StatusCode };
                }
                catch (NullReferenceException ex)
                {
                    _logger.Error("Execute Error: " + ex);
                    return new ErrorModel<string> { IsError = true, ErrorMsg = ex.ToString(), RespCode = 0 };
                }
                catch (Exception ex)
                {
                    _logger.Error("Execute Error: " + ex);
                    return new ErrorModel<string> { IsError = true, ErrorMsg = ex.ToString(), RespCode = 0 };
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Execute Error: " + ex);
                return new ErrorModel<string> { IsError = true, ErrorMsg = ex.ToString(), RespCode = 0 };
            }
        }

        private ErrorModel<string> ExecuteQuery(string requestId, string url, string data, string method)
        {
            _logger.Info("Execute: " + method + " " + url + '\n' + "   " + data);

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Timeout = HttpTimeoutMs;
                request.Method = method;
                request.ContentType = "application/json";
                request.Accept = "application/json";
                
                _logger.Info("[ExecuteQuery] Token: " + _token);

                request.ClientCertificates = _certificates;
                request.Headers.Add("Authorization", "Bearer " + _token);
                request.Headers.Add("x-ibm-client-id", _settings.ClientId);
                request.Headers.Add("RqUID", requestId);
                
                _logger.Info("[ExecuteQuery] Request headers: " + request.Headers);

                if (method == "POST")
                {
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(data);
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8))
                    {
                        var str = reader.ReadToEnd();
                        var result = new ErrorModel<string> { IsError = false, RespCode = response.StatusCode, RespObject = str };
                        _logger.Info("[Execute] Received a response: " + JsonConvert.SerializeObject(str));
                        return result;
                    }
                }

            } catch (WebException webEx)
            {
                try
                {
                    _logger.Error("[Execute] webError: " + webEx);

                    using (var reader = new StreamReader(webEx.Response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8))
                    {
                        _logger.Error("[Execute] Response: " + reader.ReadToEnd());
                    }
                    return new ErrorModel<string> { IsError = true, ErrorMsg = webEx.ToString(), RespCode = ((HttpWebResponse)webEx.Response).StatusCode };
                }
                catch (Exception ex)
                {
                    _logger.Error("[Execute] Error: " + ex);
                    return new ErrorModel<string> { IsError = true, ErrorMsg = ex.ToString(), RespCode = 0 };
                }
            }
        }
    }
}
