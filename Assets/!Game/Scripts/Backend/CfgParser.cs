/*
using System;
using System.Collections.Generic;
using System.IO;

public class CfgParser
{

    public zcfg ParseCfg(string path)
    {
        if (!File.Exists(path))
        {
            return;
        }

        zcfg returnZcfg = new zcfg();
        
        using(StreamReader sr = File.OpenText(path))
        {            
            int line = 0;
            
            while(s = sr.ReadLine != null())
            {
                s = s.ToLower(); // lower case the line to avoid case sensitivity
                s = s.Replace(" ", ""); // remove spaces to avoid needing to ever worry about them
                string[] split = s.Split("="); // split the string into two values, the key 0, and the value of said key 1

                // if we're on line one/start of file, check the magic to make sure this is a zcfg
                if (line == 0 && s != "type=zcfg")
                {
                    // throw exception for this not being a zcfg
                }

                // now that we know its a zcfg, lets read the rest of the expected header
                if (line == 1 && split[0] != "schema")
                {
                    // throw exception for expecting a schema def on line two
                }
                else
                {
                    returnZcfg.schema = split[1];
                }
                
            }
        }
    }
    
    public string AssumeType(string value)
    {
        if (value.Contains(".") && float.TryParse(value))
            return "float";
        else if (value.ToLower.Contains("false") || value.ToLower.Contains("true"))
            return "bool";
        else if (int.TryParse(value))
            return "int";
        else
            return "string"
    }
}

public class zcfg
{
    public string schema;
    public float schemaVers;

    public List<CfgKey> keys;
}

public struct CfgKey
{
    public string name;
    public string type;
    public string value;
}
*/
