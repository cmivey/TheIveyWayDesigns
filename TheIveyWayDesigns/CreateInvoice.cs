using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheIveyWayDesigns.Models;

namespace TheIveyWayDesigns
{
    public class CreateInvoice
    {
        DatabaseConnections dbConnect = new DatabaseConnections();
        public void CreatePayPalInvoice(int orderId)
        {
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);

            List<PackingSlipModel> order = dbConnect.GetPackingListInfo(orderId).ToList();

            List<InvoiceItem> items = order.Select(od => new InvoiceItem()
            {
                description = od.Description,
                name = od.Description,
                quantity = od.Quantity,
                unit_of_measure = "QUANTITY",
                unit_price = new Currency() { currency = "USD", value = od.Price.ToString() }
            }).ToList();

            var invoice = new PayPal.Api.Invoice()
            {
                merchant_info = new MerchantInfo()
                {
                    address = new InvoiceAddress()
                    {
                        city = "San Tan Valley",
                        country_code = "US",
                        state = "AZ",
                        line1 = "440 E Cayon Rock Rd",
                        postal_code = "85143",
                        phone = new Phone() { national_number = "480-326-5306", country_code = "+1" }
                    },
                    business_name = "The Ivey Way Designs",
                    email = "cmivey-facilitator@gmail.com"
                },
                billing_info = new List<BillingInfo>() { new BillingInfo()
                                                        { email = "cmivey@gmail.com",
                                                          first_name = order[0].Name.Substring(0, order[0].Name.IndexOf(' ')),
                                                          last_name = order[0].Name.Substring(order[0].Name.IndexOf(' ')),
                                                          address = new InvoiceAddress() { city = order[0].City,
                                                                                           country_code = "US",
                                                                                           state = order[0].State,
                                                                                           line1 = order[0].Address,
                                                                                           postal_code = order[0].ZipCode,
                                                                                           phone = new Phone () { national_number = order[0].PhoneNumber, country_code = "+1" } } } },
                //invoice_date = D),
                reference = "Order Number " + order[0].OrderNumber,

                shipping_cost = new ShippingCost() { amount = new Currency() { currency = "USD", value = "5.00" } },
                shipping_info = new ShippingInfo()
                {
                    address = new InvoiceAddress()
                    {
                        city = order[0].City,
                        country_code = "US",
                        state = order[0].State,
                        line1 = order[0].Address,
                        postal_code = order[0].ZipCode,
                        phone = new Phone() { national_number = order[0].PhoneNumber, country_code = "+1" }
                    },
                    first_name = order[0].Name.Substring(0, order[0].Name.IndexOf(' ')),
                    last_name = order[0].Name.Substring(order[0].Name.IndexOf(' '))
                },
                total_amount = new Currency() { currency = "USD", value = order[0].OrderTotal.ToString() },
                items = items
            };

            try
            {
                var test = invoice.Create(apiContext);
                invoice.id = test.id;
                invoice.Send(apiContext);
            }
            catch (PayPal.PaymentsException ex)
            {

            }
        }
    }
}
