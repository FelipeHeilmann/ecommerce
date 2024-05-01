namespace API;

public class Templates
{
    public static string Welcome(string name)
    {
        var template =  @"
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
}
