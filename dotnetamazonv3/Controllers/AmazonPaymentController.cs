using Com.Amazon.Pwain;
using Com.Amazon.Pwain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotnetamazonv3.Controllers
{
    public class AmazonPaymentController : Controller
    {
       private String accessKey = "AKIAJIJMTWVP5TOL7Y7Q";
       private String secretKey = "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";
       private String sellerId = "A1T5DRWMBFNP17";

        private static MerchantConfiguration merchantConfiguration;
        private static PWAINBackendSDK pwaInBackendSDK;
        private Dictionary<String, String> parameters;

        //basic form for new transaction
        public ActionResult Index()
        {
            return View();
        }

        //generate url for newtransaction
        public void NewTransaction()
        {
            String sellerOrderId;
            Random rn = new Random();
            sellerOrderId = rn.Next(1, 99999999).ToString();
            String orderTotalAmount = this.Request.Form["orderTotalAmount"];
            String orderTotalCurrencyCode = this.Request.Form["orderTotalCurrencyCode"];
            String customInformation = this.Request.Form["customInformation"];


            parameters = new Dictionary<string, string>(6);
            parameters.Add(PWAINConstants.SELLER_ORDER_ID, sellerOrderId);
            parameters.Add(PWAINConstants.ORDER_TOTAL_AMOUNT, orderTotalAmount);
            parameters.Add(PWAINConstants.ORDER_TOTAL_CURRENCY_CODE, orderTotalCurrencyCode);
            parameters.Add(PWAINConstants.REDIRECT_URL, "http://localhost:53191/AmazonPayment/TransactionResponse");
            /*Set optional parameters*/
            //For testing in Sandbox mode, "false" when going live
            parameters.Add(PWAINConstants.IS_SANDBOX, "true");
            //Transaction timeout in seconds
            parameters.Add(PWAINConstants.TRANSACTION_TIMEOUT, "1000");
            parameters.Add(PWAINConstants.CUSTOM_INFORMATION, customInformation);

            merchantConfiguration = new MerchantConfiguration.Builder().WithSellerIdValue(sellerId).WithAwsAccessKeyIdValue(accessKey).WithAwsSecretKeyIdValue(secretKey).build();
            pwaInBackendSDK = new PWAINBackendSDK(merchantConfiguration);
            String url = pwaInBackendSDK.GetPaymentUrl(parameters);

            ViewBag.Message = url;

            Response.Redirect(url);
        }

        //shows response of the transaction
        public String TransactionResponse()
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