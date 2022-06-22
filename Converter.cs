using System.Text;

namespace BasicChatApi; 

public static class Converter {
        
    public static string Base64Encode(string plainText) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        
    public static string Base64Decode(string base64EncodedData) => 
        Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));

}