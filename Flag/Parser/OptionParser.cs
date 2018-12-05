using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Flag.Parser
{

    public class Options
    {
        // Klasse for å håndtere parsinga

            // Testing GitHub

        class BoolOptions
        {
            public string shortname;
            public string longname;
            public bool isActive;            
        }

        class StringOptions
        {
            public string shortname;
            public string longname;
            public List<string> optionstr = new List<string>();
        }

        List<BoolOptions> bo = new List<BoolOptions>();
        List<StringOptions> so = new List<StringOptions>();


        public void AddStringOpt(string flags)
        {
            // Oppretter ny StringOpions med lange og korte flaggnavn

            StringOptions stro = new StringOptions();

            string[] f = flags.Split(" ");


            if (f.Length == 1)
            {
                stro.shortname = f[0];
                stro.longname = "";
            }
            if (f.Length == 2)
            {
                stro.shortname = f[0];
                stro.longname = f[1];
            }

            so.Add(stro);

        }


        public void AddBoolOpt(string flags)
        {
            // Oppretter ny BoolOpions med lange og korte flaggnavn

            BoolOptions strb =  new BoolOptions();
            
            string[] f = flags.Split(" ");
            if (f.Length == 1)
            {
                strb.shortname = f[0];
                strb.longname = "";
            }

            if (f.Length == 2)
            {
                strb.shortname = f[0];
                strb.longname = f[1];
            }
            
            strb.isActive = false;

            bo.Add(strb);
        }


        public void UpdateStrOpt(string flag_type, string flag_param)
        {
            // Legger til ny string parameter for flag_type 

            foreach (var item in so)
            {
                if (item.shortname.Equals(flag_type) || item.longname.Equals(flag_type))
                {
                    item.optionstr.Add(flag_param);
                }                
            }
        }


        public void UpdateBoolOpt(string flag_type)
        {
            //  Aktiverer valgt flag_type 

            foreach (var item in bo)
            {
                if (item.shortname.Equals(flag_type) || item.longname.Equals(flag_type))
                {
                    item.isActive = true;
                }                            
            }
        }

        public void ResetAllFlagValues()
        {
            // Nullstiller alle flagg, bool og string 

            foreach (var item in so)
            {
                item.optionstr.Clear();
            }

            foreach (var item in bo)
            {
                item.isActive = false;
            }
        }


        public bool IsSet(string flag_type)
        {
            // Returnerer True dersom flagg_type er satt, ellers False
            
            foreach (var item in so)
            {

                if (item.shortname.Equals(flag_type) || item.longname.Equals(flag_type))
                {
                    if (item.optionstr.Count != 0)
                    {
                        return true;
                    }
                }          
            }

            foreach (var item in bo)
            {
                if (item.shortname.Equals(flag_type) || item.longname.Equals(flag_type))
                {
                    return item.isActive;
                }                
            }

            return false; //default         
        }


        public string GetFirstStrOpt(string flag_type)
        {
            // Returnerer første string argument for flag_type

            string[] ret_str = new String[] { };

            foreach (var item in so)
            {
                if (item.shortname.Equals(flag_type) || item.longname.Equals(flag_type))
                {
                    ret_str = item.optionstr.ToArray();
                }                
            }

            if (ret_str.Length != 0)
            {
                return ret_str[0];
            }

            return null;  // default
        }


       
        public string[] GetAllStrOpt(string flag_type)
        {
            // Returnerer alle string argument for flag_type

            string[] ret_str = new String[] { };
            
            foreach (var item in so)
            {
                
                if (item.shortname.Equals(flag_type) || item.longname.Equals(flag_type))
                {

                    ret_str = item.optionstr.ToArray();
                }
            }

            return ret_str;
        }


        public int IndexStringFlag(string param)
        {
            //
            // Returnerer lengden på flagget som ble funnet, 0 dersom ikke funnet
            //
            var best_match = new StringOptions();
            int best_length = 0;
            
            // Sjekk mot longname først                
            foreach (var item in so)
            {
                if (param.StartsWith(item.longname) && item.longname != "")
                {
                    if (item.longname.Length > best_length)
                    {
                        best_length = item.longname.Length;
                        best_match = item;
                    }                    
                }
            }

            if (best_length != 0)
            {
                return best_match.longname.Length;
            }    

            // Ikke funnet som longname, sjekk mot shortname
            foreach (var item in so)
            {
                if (param.StartsWith(item.shortname) && item.shortname != "")
                {
                    if (item.shortname.Length > best_length)
                    {
                        best_length = item.shortname.Length;
                        best_match = item;
                    }            
                }                
            }

            if (best_length != 0)
            {
                return best_match.shortname.Length;                
            }

            return 0;  // Default

        }

        public int IndexBoolFlag(string param)
        {
            //
            // Returnerer lengden på flagget som er funnet
            //
            var best_match = new BoolOptions();
            int best_length = 0;

            // Sjekk mot longname først                
            foreach (var item in bo)
            {
                if (param.StartsWith(item.longname) && item.longname != "")
                {
                    if (item.longname.Length > best_length)
                    {
                        best_length = item.longname.Length;
                        best_match = item;
                    }

                    return item.longname.Length;
                }
            }

            if (best_length != 0)
            {
                return best_match.longname.Length;
            }

            // Ikke funnet som longname, sjekk mot shortname
            foreach (var item in bo)
            {
                if (param.StartsWith(item.shortname) && item.shortname != "")
                {
                    if (item.shortname.Length > best_length)
                    {
                        best_length = item.shortname.Length;
                        best_match = item;
                    }                    
                }                
            }
            if (best_length != 0)
            {
                return best_match.shortname.Length;                
            }

            return 0; //default         
        }

        public string[] Parse(string[] argv, Options opt)
        {
            /* Fotutsettninger jeg har tatt angående Parse() rutina
             *  
             * ----------------------------------------------------
             * - Alle String Options har en streng-parameter i tillegg til flagget
             * - Om argv[i] inneholder kun ett flagg, ligger parameter i neste argv[]
             * - Om argv[i] inneholder mer enn selve flagget, ligger parameter bak flagget i samme argv[]
             * - Eventuelle '=' tegn i parameter ignoeres, og erstattes med  ' ' tegn ved pasring.
             *  
             *   Boolean Options er kun flagg, ingen parameter i tillegg til flagget
             * 
             */


            List<string> retur_str = new List<string>();
            // string retur_str = "";            
            string argument = "";
            string last_flag = "";
            bool bool_flag = false;
            int flag_index = 0;

            string expected_parameter = "flag";  // Første forventede parameter som skal behandles er et flag

            for (int i = 0; i < argv.Length; i++)
            {
                argument = argv[i];

                switch (expected_parameter)
                {
                    case "flag":
                        {
                            if (!(argument.StartsWith('-')))
                            {
                                // Parameter er ikke et flagg, ignorer og legg det i returstrengen dersom ikke BLANKT                           
                                // Markerer at argument ikke er behandlet?
                                if (argument.Trim(' ') != "")
                                {
                                    retur_str.Add(argv[i]);
                                    // retur_str += argv[i] + " ";
                                }
                                break;
                            }

                            // Behandle flag                            
                            // Fjern alle '-' tegn fra argument
                            argument = argument.Replace("-", "");
                            // Estatter alle = med  SPACE
                            argument = argument.Replace("=", " ");

                            // Sjekk om streng flagg først
                            flag_index = opt.IndexStringFlag(argument);

                            // Sjekk om bool flag dersom ikke streng flag
                            if (flag_index == 0)
                            {
                                flag_index = opt.IndexBoolFlag(argument);
                                bool_flag = flag_index != 0;

                                if (bool_flag)
                                {
                                    opt.UpdateBoolOpt(argument.Substring(0, flag_index));

                                    // Sjekk om flere bool flag inkludert juxtaposition
                                    int current_flag_index = 0;

                                    while ((argument.Length - flag_index > 0) && flag_index != 0)
                                    {
                                        current_flag_index += flag_index;
                                        flag_index = opt.IndexBoolFlag(argument.Substring(current_flag_index));
                                        if (flag_index != 0)
                                        {
                                            opt.UpdateBoolOpt(argument.Substring(current_flag_index, flag_index));
                                        }

                                    }                             
                                }
                            }
                            
                             
                            // Håndter string flag
                            if (flag_index != 0 && !(bool_flag))  //  streng flag
                            {
                                if (flag_index == argument.Length)
                                {
                                    // Parameter forventes i neste argument
                                    expected_parameter = "parameter";                            
                                }
                                else
                                {
                                    // Argument ligger i samme argumet, bak flagget 
                                    opt.UpdateStrOpt(argument.Substring(0, flag_index), argument.Substring(flag_index).Trim());
                                }
                                last_flag = argument.Substring(0, flag_index);
                            }
                            else
                            {
                                if (!bool_flag)
                                    Console.WriteLine("Flag ikke funnet : " + argument);
                                else
                                    bool_flag = false;
                            }

                            break;
                        }

                    case "parameter":
                        {
                            if (argument.Trim(' ') != "")  // Ignorer eventuell blank parameter. Forvent parameter i neste argument
                            {
                                opt.UpdateStrOpt(last_flag, argument.Trim());
                                expected_parameter = "flag";
                            }
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Hit skal man aldri komme");
                            break;
                        }
                }
            }

            // return retur_str.TrimEnd().Split(" ");
            return retur_str.ToArray();
        }
    }

    public class OptionParser
    {

        Options opt = new Options();

        public void AddStringOption(string flags)
        {
            // TODO - add white space separated flag list (long and short flags)
            opt.AddStringOpt(flags);
        }

        public void AddBoolOption(string flags)
        {
            // TODO - add white space separated flag list (long and short flags)
            opt.AddBoolOpt(flags);
        }

        public bool IsSet(string flag)
        {
            // TODO - return true if a flag (bool or string) was set
            return opt.IsSet(flag);
        }


        public string[] Parse(string[] argv)
        {
            // TODO - parse command line arguments
            return opt.Parse(argv, opt);            
        }

        public string Get(string flag)
        {
            // TODO - return first string argument for flag
            return opt.GetFirstStrOpt(flag);
        }

        public string[] GetAll(string flag)
        {
            // TODO - return all string arguments for flag
            return opt.GetAllStrOpt(flag);
        }

        public void Reset()
        {
            // TODO  - unsets all flags
            opt.ResetAllFlagValues();
        }
    }
}