/* Copyright (c) 2011 Chris Chen. 
 * All rights reserved. This program and the accompanying materials 
 * are made available under the terms of the Eclipse Public License v1.0 
 * which accompanies this distribution, and is available at 
 * http://www.eclipse.org/legal/epl-v10.html
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace Solvet
{
    class Calculation
    {
        public Calculation(string query)
        {
            Query = query;

            string url = "http://www" + Properties.Settings.Default.TLD + "/ig/calculator?q=" + Utils.RemoveSpacesFromNumbers(Query.Replace("+", "%2B"));

            string result = "";

            try
            {
                WebClient client = new WebClient();
                result = client.DownloadString(url);

            }
            catch (Exception ex)
            {
                // handle error
                Console.WriteLine(ex.Message);
            }

            Input = Utils.CleanHTML(Utils.ExtractString("{lhs: \"", "\"", result));
            Output = Utils.CleanHTML(Utils.ExtractString(",rhs: \"", "\"", result));
            ErrorText = Utils.CleanHTML(Utils.ExtractString(",error: \"", "\"", result));

            this.Error = !(ErrorText == "0" || ErrorText == "");

            if (ErrorText == "4" || ErrorText == "3")
            {
                ErrorText = "I don't understand.";
            }
        }

        public string Query { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string ErrorText { get; set; }
        public bool Error { get; set; }

    }
}
