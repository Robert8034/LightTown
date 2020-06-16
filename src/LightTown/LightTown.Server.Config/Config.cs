using System;
using System.IO;
using Newtonsoft.Json;

namespace LightTown.Server.Config
{
    public class Config
    {
        private static bool _configLoaded = false;

        private static string _lightTownHost;
        private static string _smtpServerHost;
        private static int _smtpServerPort;
        private static bool _smtpServerEnableSsl;
        private static string _smtpServerUsername;
        private static string _smtpServerPassword;
        private static string _smtpServerEmail;
        private static string _postgresHost = "localhost";
        private static int _postgresPort = 5432;
        private static string _postgresDatabase;
        private static string _postgresUserId;
        private static string _postgresPassword;

        [JsonProperty]
        public static string UserAvatarPath = Path.Combine(AppContext.BaseDirectory, "avatars", "users");
        [JsonProperty]
        public static string ProjectImagePath = Path.Combine(AppContext.BaseDirectory, "images", "projects");
        
        [JsonProperty]
        public static string LightTownHost
        {
            get => _lightTownHost;
            set
            {
                _lightTownHost = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static string SmtpServerHost
        {
            get => _smtpServerHost;
            set
            {
                _smtpServerHost = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static int SmtpServerPort
        {
            get => _smtpServerPort;
            set
            {
                _smtpServerPort = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static bool SmtpServerEnableSsl
        {
            get => _smtpServerEnableSsl;
            set
            {
                _smtpServerEnableSsl = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static string SmtpServerUsername
        {
            get => _smtpServerUsername;
            set
            {
                _smtpServerUsername = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static string SmtpServerPassword
        {
            get => _smtpServerPassword;
            set
            {
                _smtpServerPassword = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static string SmtpServerEmail
        {
            get => _smtpServerEmail;
            set
            {
                _smtpServerEmail = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static string PostgresHost
        {
            get => _postgresHost;
            set
            {
                _postgresHost = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static int PostgresPort
        {
            get => _postgresPort;
            set
            {
                _postgresPort = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static string PostgresDatabase
        {
            get => _postgresDatabase;
            set
            {
                _postgresDatabase = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static string PostgresUserId
        {
            get => _postgresUserId;
            set
            {
                _postgresUserId = value;

                SaveConfig();
            }
        }
        [JsonProperty]
        public static string PostgresPassword
        {
            get => _postgresPassword;
            set
            {
                _postgresPassword = value;

                SaveConfig();
            }
        }

        /// <summary>
        /// Checks whether the config properties are valid and log to the console if any are not.
        /// <para>
        /// It is highly recommended to NOT start the server if this method returns false.
        /// </para>
        /// </summary>
        /// <returns>Returns false if any config properties are invalid.</returns>
        public static bool CheckConfig()
        {
            if (string.IsNullOrEmpty(LightTownHost))
            {
                LogEmptyProperty("LightTownHost");
                return false;
            }

            if (string.IsNullOrEmpty(SmtpServerHost))
            {
                LogEmptyProperty("SmtpServerHost");
                return false;
            }

            if (SmtpServerPort < 255 || SmtpServerPort > 65535)
            {
                LogInvalidProperty("SmtpServerPort", SmtpServerPort.ToString(), "Port should be between 255 and 65535");
                return false;
            }

            if (string.IsNullOrEmpty(SmtpServerUsername))
            {
                LogEmptyProperty("SmtpServerUsername");
                return false;
            }

            if (string.IsNullOrEmpty(SmtpServerPassword))
            {
                LogEmptyProperty("SmtpServerPassword");
                return false;
            }

            if (string.IsNullOrEmpty(SmtpServerEmail))
            {
                LogEmptyProperty("SmtpServerEmail");
                return false;
            }

            if (string.IsNullOrEmpty(PostgresHost))
            {
                LogEmptyProperty("PostgresHost");
                return false;
            }

            if (PostgresPort < 255 || PostgresPort > 65535)
            {
                LogInvalidProperty("PostgresPort", PostgresPort.ToString(), "Port should be between 255 and 65535");
                return false;
            }

            if (string.IsNullOrEmpty(PostgresDatabase))
            {
                LogEmptyProperty("PostgresDatabase");
                return false;
            }

            if (string.IsNullOrEmpty(PostgresUserId))
            {
                LogEmptyProperty("PostgresUserId");
                return false;
            }

            if (string.IsNullOrEmpty(PostgresPassword))
            {
                LogEmptyProperty("PostgresPassword");
                return false;
            }

            if (!Uri.TryCreate(LightTownHost, UriKind.Absolute, out var uriResult) ||
                uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            {
                LogInvalidProperty("LightTownHost", LightTownHost, 
                    "The LightTown host needs to be a valid http/https URL ending with a \"/\", example (without quotes): \"https://example.com/\"");
                return false;
            }

            if (!LightTownHost.EndsWith("/"))
            {
                LogInvalidProperty("LightTownHost", LightTownHost,
                    "The LightTown host needs to end with a \"/\", example (without quotes): \"https://example.com/\"");
                return false;
            }

            try
            {
                //check if the email is valid
                if (new System.Net.Mail.MailAddress(SmtpServerEmail).Address != SmtpServerEmail)
                {
                    LogInvalidProperty("SmtpServerEmail", SmtpServerEmail);
                    return false;
                }
            }
            catch (Exception)
            {
                LogInvalidProperty("SmtpServerEmail", SmtpServerEmail);
                return false;
            }

            return true;
        }

        private static void LogEmptyProperty(string property)
        {
            Console.WriteLine("Error loading config file:");
            Console.WriteLine($"Property \"{property}\" is empty.");
        }

        private static void LogInvalidProperty(string property, string value, string reason = null)
        {
            Console.WriteLine("Error loading config file:");
            Console.WriteLine($"Property \"{property}\" is invalid, value: {value}");

            if(!string.IsNullOrEmpty(reason))
                Console.WriteLine($"Reason: {reason}");
        }

        public static bool LoadConfig()
        {
            string configPath = Path.Combine(AppContext.BaseDirectory, "config.json");

            bool valid = File.Exists(configPath);

            if (valid)
            {
                JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
            }

            _configLoaded = true;

            if (!valid)
            {
                Console.WriteLine("No config could be found so a new one has been created. Please fill in the config file with the correct data and start the server again.");
                Console.WriteLine("The config file is located in the application directory with the name \"config.json\".");
                Console.WriteLine("After starting the server again you will be notified if any properties are invalid.");

                SaveConfig();
            }

            return valid;
        }

        public static void SaveConfig()
        {
            //don't save when the config hasn't loaded yet, this makes sure SaveConfig() doesn't get called when deserializing the JSON file.
            if (!_configLoaded)
                return;

            string configPath = Path.Combine(AppContext.BaseDirectory, "config.json");

            string json = JsonConvert.SerializeObject(new Config(), Formatting.Indented);
            
            File.WriteAllText(configPath, json);
        }
    }
}
