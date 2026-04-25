
namespace Clinic_System.Application.Common
{
    public static class EmailTemplates
    {
        public static string GetEmailConfirmationTemplate(string name, string userName, string email, string confirmationLink, string userRole, string? specialty = null)
        {
            // 1. تحديد نوع الحساب ورسالة الترحيب بناءً على وجود التخصص
            bool isDoctor = userRole == "Doctor";
            string accountType = isDoctor ? "Doctor" : "Patient";
            string welcomeSubtitle = isDoctor ? "Welcome to our Medical Team 👨‍⚕️" : "Welcome to our family 🏡";

            // 2. تجهيز سطر التخصص (يظهر فقط للدكتور)
            string specialtyRow = "";
            if (!string.IsNullOrEmpty(specialty) && isDoctor)
            {
                specialtyRow = $@"<div class='details-item'><strong>Specialty:</strong> {specialty}</div>";
            }

            // 3. تعديل الاسم (لو دكتور نضيف Dr. قبل الاسم)
            string displayName = isDoctor && !name.StartsWith("Dr.", StringComparison.OrdinalIgnoreCase)
                ? $"Dr. {name}"
                : name;

            return $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f6f8; margin: 0; padding: 0; }}
                .container {{ max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 10px; box-shadow: 0 4px 8px rgba(0,0,0,0.05); overflow: hidden; border: 1px solid #e1e4e8; }}
                .header {{ background-color: #0d6efd; color: #ffffff; padding: 30px; text-align: center; }}
                .header h1 {{ margin: 0; font-size: 24px; font-weight: 600; }}
                .content {{ padding: 30px; color: #333333; line-height: 1.6; }}
                .details-box {{ background-color: #f8f9fa; border-left: 5px solid #0d6efd; padding: 15px; margin: 20px 0; border-radius: 4px; }}
                .details-item {{ margin-bottom: 10px; font-size: 14px; }}
                .btn-container {{ text-align: center; margin-top: 30px; margin-bottom: 20px; }}
                .btn {{ background-color: #0d6efd; color: #ffffff !important; text-decoration: none; padding: 12px 30px; border-radius: 5px; font-weight: bold; font-size: 16px; display: inline-block; transition: background-color 0.3s; }}
                .btn:hover {{ background-color: #0b5ed7; }}
                .footer {{ background-color: #f8f9fa; padding: 20px; text-align: center; font-size: 12px; color: #6c757d; border-top: 1px solid #e1e4e8; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Elite Clinic System</h1>
                    <p style='margin: 5px 0 0; opacity: 0.9;'>{welcomeSubtitle}</p>
                </div>

                <div class='content'>
                    <h2 style='color: #2c3e50; margin-top: 0;'>Hi, {displayName}! 👋</h2>
                    <p>Thanks for joining Elite Clinic. We are excited to have you on board.</p>
                    
                    <p>Here is a summary of your registration details:</p>
                    
                    <div class='details-box'>
                        <div class='details-item'><strong>User Name:</strong> {userName}</div>
                        <div class='details-item'><strong>Email Address:</strong> {email}</div>
                        <div class='details-item'><strong>Account Type:</strong> {accountType}</div>
                        {specialtyRow}
                    </div>

                    <p>Please confirm your email address to activate your account.</p>

                    <div class='btn-container'>
                        <a href='{confirmationLink}' class='btn'>Confirm My Account</a>
                    </div>
                    
                    <p style='font-size: 13px; color: #999;'>If the button doesn't work, copy and paste this link into your browser:<br>
                    <a href='{confirmationLink}' style='color: #0d6efd; word-break: break-all;'>{confirmationLink}</a></p>
                </div>

                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} Elite Clinic. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>";
        }
    }
}
