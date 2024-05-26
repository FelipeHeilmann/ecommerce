using API.Events;
using API.Model;
using API.Request;
using System.Text;

namespace API;

public class Templates
{
    public static string Welcome(string name)
    {
        string template =  @"
            <html>
            <head>
                <style>
                    /* Define your CSS styles here */
                    /* Example styles */
                    body {
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                    }
                    .container {
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        border: 1px solid #ccc;
                        border-radius: 10px;
                    }
                    .footer {
                        margin-top: 20px;
                        font-size: 12px;
                        color: #666;
                    }
                    .button {
                        display: inline-block;
                        padding: 10px 20px;
                        background-color: #007bff;
                        color: #fff;
                        text-decoration: none;
                        border-radius: 5px;
                    }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Welcome!</h1>
                    <p>Dear [User],</p>
                    <p>Welcome to our e-commercce! We're excited to have you join our community of savvy shoppers.</p>
                    <p>As a new member, you now have access to exclusive benefits:</p>
                    <ul>
                        <li>Receive personalized product recommendations based on your interests and browsing history.</li>
                        <li>Enjoy special discounts and promotions available only to our loyal customers.</li>
                        <li>Get early access to new arrivals and limited-time offers.</li>
                    </ul>
                    <p>To start shopping, simply browse our wide selection of products and add your favorites to your cart. Don't forget to check out our latest deals and featured collections!</p>
                    <p>We're here to make your shopping experience seamless and enjoyable. If you have any questions or need assistance, feel free to reach out to our customer support team at [support email or link to support page].</p>
                    <p>Happy shopping!</p>
                    <a href='[Link to your website]' class='button'>Start Shopping</a>
                    <div class='footer'>
                        <p>Best regards,<br/>Felipe Heilmann<br/>CTO<br/>e-commercce</p>
                    </div>
                </div>
            </body>
            </html>
            ";

        template = template.Replace("[User]", name);
        return template;
    }
    public static string OrderCheckedout(OrderCheckedoutMailModel order)
    {
        string template = @"
            <html>
            <head>
                <style>
                    /* Define your CSS styles here */
                    /* Example styles */
                    body {
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                    }
                    .container {
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        border: 1px solid #ccc;
                        border-radius: 10px;
                    }
                    .order-details {
                        margin-bottom: 20px;
                    }
                    .order-item {
                        margin-bottom: 10px;
                    }
                    .footer {
                        margin-top: 20px;
                        font-size: 12px;
                        color: #666;
                    }
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Order Confirmation</h1>
                    <p>Dear [User],</p>
                    <p>Thank you for your order with our e-commercce! Your order details are as follows:</p>
                    <div class='order-details'>
                        <p><strong>Order Number:</strong> #[OrderNumber]</p>
                        <p><strong>Order Date:</strong> [OrderDate]</p>
                    </div>
                    <p><strong>Order Items:</strong></p>
                    <ul class='order-items'>
                        [OrderItems]
                    </ul>
                    <p><strong>Total Amount:</strong> $[TotalAmount]</p>
                    <p>Your order is currently being processed. You will receive another email once your order has been shipped.</p>
                    <p>Thank you for shopping with us!</p>
                    <div class='footer'>
                        <p>Best regards,<br/>Felipe Heilmann<br/>CTO<br/>e-commercce</p>
                    </div>
                </div>
            </body>
            </html>
        ";

        template = template.Replace("[User]", order.Name);
        template = template.Replace("[OrderNumber]", order.OrderId.ToString());
        template = template.Replace("[OrderDate]", order.Date.ToString("MM/dd/yyyy"));
        template = template.Replace("[TotalAmount]", order.Items.Sum(item => item.Quantity * item.Price).ToString("0.00"));
        StringBuilder itemsHtml = new StringBuilder();

        foreach (var item in order.Items)
        {
            itemsHtml.Append("<li class='order-item'>");
            itemsHtml.Append($"<strong>{item.Name}</strong> - ${item.Price} x {item.Quantity}");
            itemsHtml.Append("</li>");
        }
        template = template.Replace("[OrderItems]", itemsHtml.ToString());
        return template;
    }
}
