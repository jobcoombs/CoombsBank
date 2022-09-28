using System.Text.Json.Serialization;

namespace CoombsBank.Constants;

public class FirebaseSettings
{
    [JsonPropertyName("project_id")]
    public string ProjectId => "coombsbank";

    [JsonPropertyName("private_key_id")]
    public string PrivateKeyId => "4894423e2eb9bba1dc9c2e03917e09f52e263f7b";

    [JsonPropertyName("type")]
    public string Type => "service_account";
    
    [JsonPropertyName("private_key")]
    public string PrivateKey => "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCt6nFHF9BQwevG\nn2PCxK0tha05ZVf5zZYjqg44//v5zKV28XODzm3iw9obZdQQ69SXL48x+63r+NtX\nqmVp1gfXVtvCF+pqJdmG8vOBKt0sCMlL7PrXc8XlyHJlMIvhgv7s1cFYMgkjAQU/\nlgEgWka7pDXGRcoMtV3EqUOrupI+ShtpvEcmB9OipxRBsnTtlu0ep7ReNfapxU0B\nYMgrWry0TfWypYUapglVIh2pBxDw5cKqQOXwl+e8hdHd4SDAqNlFyPDngTRWC5he\nBo92zfM2xZWOJcqclTqPrmARbFIrn+mJw4ngCAZCJLZ+wkyVgGTJupELfXv5P9dB\n7CcdnlmhAgMBAAECggEAAPpgpyZAzRjL3Pn6DDOTnUxBfFolN0nczJ82UCcU0yAG\nuGkS+jqrS07W4GKRNtleclunx0a8gOA384/swIWBgDou7msOAzPqnyPFhMtwlPmX\nOuBpS9Q3n7Ch4yuR8V4qAC1octVNM6FNul0Je3k0t4Pej1NxXqoJjfU9SEud+OzO\nR8CL6GJui/4JwU4k1p+m6SxEOKeH01i7gg1TcYy3cCCo4zIJmQVvF69zIjUIcb50\n1nGM60h5CrIyh6oW/lL8pnCXHEMLKiSZbzLIGm2WJtr0J2W0tNb9yQ3PW5cglOro\nNigqpBLc6ZaLv2Uxjl6YHw2JnTmZT0FJQd16iDsMtQKBgQDU4WghFaRpf2YsHMbc\nkwpQh8JB4ZhM9JFGqrdH5RPEp3fK3hLvqE936TY/VtgV7cRnS4HEQnGo4SXp9a+u\nlL05YxBnPkhJOJ2vHBjCxUfkF0Jg8c7uX4NhoT+HMp4+kW08q5ZNaVco74BwJ3QP\nUIcZ2debTPvD9ClaCy1M4hNNpwKBgQDRJJSY4DmN9mSBzItU1HV5DgNtXuuWk2XA\nVU+hUOTOjzMoE0sbJ9bWGr05ZecPno8WDnF4UAdNjs1XhoSmcT1zhT7U26rEyDv2\nNalxF3XB8l3/zv0Hslvj2/WzPO8kyCpT1kX/GC77ufk/DMrWscSMPk4VPlH1yWa1\nEBu7BeLXdwKBgQDR/H6AlB6Xygl43fHzRj/Ya/HclrFqx5w9/svH6Vl0lFUAEcrQ\nCFBLUgomtH12NBGWas7YT0pacay6eZUQOrvkzakyjiczwtsYDjkGdx+RCLh7dErQ\nq/qm5S4LKI0b3svIGuGp/ozxXj5tYx2CtcZuY4ngMTHvevsHTRskSaUM5QKBgGiV\n921fq8hvL6GtLPv15uzrchFOh1dcTGTbBKAx+MnG2Jkw1L02tfuJkZjTebHY48ru\n3BWDR7OkNtGPJmSpekZ7mokNdJ9FVp7wBNTWG4AqeJFE7EneCo+u3naMgTaOloQo\nQgaoDE67mIXMGt80NIrBWYDMEmZsGd1vub5T2jVPAoGAV99QuHO6Rc8sKo6wwb9g\ny4uagn4c9A324YvxqCsPYYArQzqCw+8WFuh1zDbq2tBMBvuEU2rp6FiGur8LlQ5a\nKeRckwOntGJfqbocGFFkI0T50Ou7E4sqtFesX2TCn19yzSKRPrVQosa2Nta++Vx3\nyYMa05P+lW9SunC6QNpYmpU=\n-----END PRIVATE KEY-----\n";
    
    [JsonPropertyName("client_email")]
    public string ClientEmail => "firebase-adminsdk-faumf@coombsbank.iam.gserviceaccount.com";
    
    [JsonPropertyName("client_id")]
    public string ClientId => "102603367996391740644";
    
    [JsonPropertyName("auth_uri")]
    public string AuthUri => "https://accounts.google.com/o/oauth2/auth";
    
    [JsonPropertyName("token_uri")]
    public string TokenUri => "https://oauth2.googleapis.com/token";
    
    [JsonPropertyName("auth_provider_x509_cert_url")]
    public string AuthProvider => "https://www.googleapis.com/oauth2/v1/certs";
    
    [JsonPropertyName("client_x509_cert_url")]
    public string ClientCertUrl => "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-faumf%40coombsbank.iam.gserviceaccount.com";
}