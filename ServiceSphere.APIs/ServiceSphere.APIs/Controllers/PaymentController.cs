using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace ServiceSphere.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> {
                "card",
            },
                LineItems = new List<SessionLineItemOptions> {
                new SessionLineItemOptions {
                    PriceData = new SessionLineItemPriceDataOptions {
                        UnitAmount = 2000, // Price in cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions {
                            Name = "Freelance service",
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = "https://yourdomain.com/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://yourdomain.com/cancel",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            return Ok(new { Url = session.Url });
        }

        [HttpPost("create-checkout-sessionF")]
        public ActionResult CreateCheckoutSessionF(string freelancerStripeAccountId)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 2000, // Price in cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Freelance service",
                    },
                },
                Quantity = 1,
            },
        },
                Mode = "payment",
                SuccessUrl = "https://yourdomain.com/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://yourdomain.com/cancel",
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    // Optionally specify that the charge collected should be transferred to the connected account
                    TransferData = new SessionPaymentIntentDataTransferDataOptions
                    {
                        Destination = freelancerStripeAccountId,
                    },
                },
            };
            var service = new SessionService();
            Session session = service.Create(options);

            return Ok(new { Url = session.Url });
        }

    }
}
