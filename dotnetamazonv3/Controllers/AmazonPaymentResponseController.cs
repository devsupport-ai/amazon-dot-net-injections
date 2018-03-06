using Com.Amazon.Pwain;
using Com.Amazon.Pwain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace {{=it.project_name}}.Controllers
{
    public class AmazonPaymentResponseController : Controller
    {
        private String sellerId = "{{=it.merchant_id}}";
        private String accessKey = "{{=it.access_key}}";
        private String secretKey = "{{=it.secret_key}}";

        private static MerchantConfiguration merchantConfiguration;
        private static PWAINBackendSDK pwaInBackendSDK;
        private Dictionary<String, String> parameters;


        // GET: AmazonPaymentResponse
        public String Index()
        {

            try
            {
                string sellerOrderId = this.Request.QueryString["sellerOrderId"];
                string orderTotalAmount = this.Request.QueryString["orderTotalAmount"];
                string orderTotalCurrencyCode = this.Request.QueryString["orderTotalCurrencyCode"];
                string reasonCode = this.Request.QueryString["reasonCode"];
                string amazonOrderId = this.Request.QueryString["amazonOrderId"];
                string signature = this.Request.QueryString["signature"];
                string status = this.Request.QueryString["status"];
                string transactionDate = this.Request.QueryString["transactionDate"];
                string description = this.Request.QueryString["description"];
                String customInformation = this.Request.QueryString["customInformation"];

                //Add the response parameter values to a Dictionary
                parameters = new Dictionary<string, string>(6);
                parameters.Add(PWAINConstants.SELLER_ORDER_ID, sellerOrderId);
                parameters.Add(PWAINConstants.ORDER_TOTAL_AMOUNT, orderTotalAmount);
                parameters.Add(PWAINConstants.ORDER_TOTAL_CURRENCY_CODE, orderTotalCurrencyCode);
                parameters.Add(PWAINConstants.REASON_CODE, reasonCode);
                parameters.Add(PWAINConstants.AMAZON_ORDER_ID, amazonOrderId);
                parameters.Add(PWAINConstants.VERIFY_SIGNATURE, signature);
                parameters.Add(PWAINConstants.STATUS, status);
                parameters.Add(PWAINConstants.TRANSACTION_DATE, transactionDate);
                parameters.Add(PWAINConstants.DESCRIPTION, description);
                parameters.Add(PWAINConstants.CUSTOM_INFORMATION, customInformation);

                merchantConfiguration = new MerchantConfiguration.Builder().WithSellerIdValue(sellerId).WithAwsAccessKeyIdValue(accessKey).WithAwsSecretKeyIdValue(secretKey).build();
                pwaInBackendSDK = new PWAINBackendSDK(merchantConfiguration);

                pwaInBackendSDK.VerifySignature(parameters);
                return string.Join(";", parameters);
                //Signature verification successful please process the order
            }
            catch (Exception e)
            {
                return "verification failed:" + e.StackTrace;
                //Signature verification failed
            }
        }
    }
}