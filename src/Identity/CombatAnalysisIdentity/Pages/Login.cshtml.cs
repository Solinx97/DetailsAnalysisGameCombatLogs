using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysisIdentity.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace CombatAnalysisIdentity.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService<AppUserDto> _service;

        public LoginModel(IUserService<AppUserDto> service)
        {
            _service = service;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl, string username, string password)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return Page();
            }


            var user = await _service.GetAsync(username, password);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    var authorizationCode = GenerateAuthorizationCode(user.Id);
                    var encodedAuthorizationCode = Uri.EscapeDataString(authorizationCode);
                    var redirectUrl = $"{returnUrl}?code={encodedAuthorizationCode}&state=authorized";

                    return Redirect(redirectUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return Page();
        }

        private string GenerateAuthorizationCode(string userId)
        {
            var authorizationCode = GenerateAuthorizationCode();
            Encryption.EnctyptionKey = GenerateAesKey();

            var encryptedAuthorizationCode = EncryptAuthorizationCodeWithCustomData(authorizationCode, userId, Encryption.EnctyptionKey);

            return encryptedAuthorizationCode;
        }

        private static string GenerateAuthorizationCode()
        {
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            var randomBytes = new byte[32]; // 256 bits
            randomNumberGenerator.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        private static byte[] GenerateAesKey()
        {
            using var aes = Aes.Create();
            aes.GenerateKey();

            return aes.Key;
        }

        private static string EncryptAuthorizationCodeWithCustomData(string authorizationCode, string customData, byte[] encryptionKey)
        {
            string combinedData = $"{authorizationCode}:{customData}";

            using var aes = Aes.Create();
            aes.Key = encryptionKey;
            aes.GenerateIV();

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(combinedData);
            }

            var iv = aes.IV;
            var encrypted = ms.ToArray();

            var result = new byte[iv.Length + encrypted.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

            var encryptedAuthorizationKey = Convert.ToBase64String(result);

            return encryptedAuthorizationKey;
        }
    }
}
