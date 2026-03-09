using UnityEngine;
using TMPro;
using System.IO;

namespace Bastion.LocalizationEditor
{
    public class FileEditor : MonoBehaviour
    {
        //References
        [SerializeField] TMP_InputField inputField;
        [SerializeField] TMP_Text nameBar;
       
        // fully hidden References
        EditorMain em;
        // display variables
        string fileName = "";
        // variables
        string path;
        bool unsavedChanges = false;

        public void ContactED(EditorMain em)
        {
            this.em = em;

            string encrypted = Bastion.PersistanceManager.Encrypt("{\r\n    \"firstNames\": [\r\n        \"Kathy\",\r\n        \"Brody\",\r\n        \"Jake\",\r\n        \"Tyler\",\r\n        \"Tristan\",\r\n        \"Layben\",\r\n        \"James\",\r\n        \"Dylan\",\r\n        \"Shwartz\",\r\n        \"Luigi\",\r\n        \"Eve\",\r\n        \"Evalyin\",\r\n        \"Mable\",\r\n        \"Jenny\",\r\n        \"Hellin\",\r\n        \"Stefon\",\r\n        \"Steve\",\r\n        \"Blake\",\r\n        \"Cindy\",\r\n        \"Ash\",\r\n        \"John\",\r\n        \"Terry\",\r\n        \"Louis\",\r\n        \"Marco\",\r\n        \"Peter\",\r\n        \"Francine\",\r\n        \"Bill\",\r\n        \"Zach\",\r\n        \"Stanly\",\r\n        \"Sero\",\r\n        \"Connor\",\r\n        \"LE BRON!!!\",\r\n        \"Harry\",\r\n        \"Harley\",\r\n        \"Clemen\",\r\n        \"Amy\",\r\n        \"Kyle\",\r\n        \"Jack\",\r\n        \"Mooks\",\r\n        \"Larry\",\r\n        \"Whane\",\r\n        \"Perry\",\r\n        \"Percy\",\r\n        \"Cain\",\r\n        \"Benjemen\",\r\n        \"Geoirorge\",\r\n        \"Josey\",\r\n        \"Bradey\",\r\n        \"Able\",\r\n        \"Oroburose\",\r\n        \"Pam\",\r\n        \"Sammy\",\r\n        \"Rob\",\r\n        \"Heidi\",\r\n        \"Carl\",\r\n        \"Jimmy\",\r\n        \"Alex\",\r\n        \"Igore\",\r\n        \"Drew\",\r\n        \"Paul\",\r\n        \"Kevin\"\r\n    ],\r\n    \"lastNames\": [\r\n        \"Zaliano\",\r\n        \"Houser\",\r\n        \"Wast\",\r\n        \"East\",\r\n        \"Carry\",\r\n        \"Charlston\",\r\n        \"Gobston\",\r\n        \"Hemsworth\",\r\n        \"Livington\",\r\n        \"Tyson\",\r\n        \"Archibald\",\r\n        \"Voltwin\",\r\n        \"Metro\",\r\n        \"Bucker\",\r\n        \"Swash\",\r\n        \"Gibs\",\r\n        \"Axelson\",\r\n        \"Turner\",\r\n        \"Harper\",\r\n        \"Goudy\",\r\n        \"Vargus\",\r\n        \"Carlson\",\r\n        \"Walson\",\r\n        \"Wreath\",\r\n        \"Bourque\",\r\n        \"Huple\",\r\n        \"Kipston\",\r\n        \"Grecher\",\r\n        \"Crox\",\r\n        \"Princeton\",\r\n        \"Nickolas\",\r\n        \"Preaches\",\r\n        \"Qwepton\",\r\n        \"Kerbolt\",\r\n        \"Gumbowl\",\r\n        \"Doser\",\r\n        \"Wolfeschlegelsteinhausenbergerdorff\",\r\n        \"Zamperston!\",\r\n        \"Yishton\",\r\n        \"Reparted\",\r\n        \"Cously\",\r\n        \"Venson\",\r\n        \"Lucsly\",\r\n        \"Unlucsly\",\r\n        \"Farted\",\r\n        \"Balus\",\r\n        \"Marco\",\r\n        \"Polo\",\r\n        \"Anyways\"\r\n    ],\r\n    \"comboNames\": [\r\n        \"Mozzarella Sticks\",\r\n        \"Material Gorl\",\r\n        \"George Washingmashine\",\r\n        \"Abraham Tinkoln\",\r\n        \"Chris Hancock\",\r\n        \"Harry Schmotter\",\r\n        \"Marco Marco\",\r\n        \"Louisi Marco\",\r\n        \"Donbald Trunk\",\r\n        \"Neckolas Noduro\",\r\n        \"Rob Bukowski\",\r\n        \"Rob Buks\",\r\n        \"John Chinease\",\r\n        \"Meter Parkington\",\r\n        \"Bob Rukowski\",\r\n        \"Gaben Newl\",\r\n        \"Peace Shooter\",\r\n        \"Mucg Saigok\",\r\n        \"Ron Seena\",\r\n        \"Srank Finatra\",\r\n        \"Mabby Hick\",\r\n        \"Fonald Rowers\",\r\n        \"Tayble Kloff\",\r\n        \"Michkal Wave\",\r\n        \"Guy McGuyson II\",\r\n        \"Heydro Pastel\",\r\n        \"Taco Well\",\r\n        \"Ecks Comm II\",\r\n        \";\",\r\n        \"Paan Cayk\",\r\n        \"Tomas Tanck\",\r\n        \"Lukas Wast\",\r\n        \"Loogle Argo\",\r\n        \"Coal Haus\",\r\n        \"Rink  Rof\",\r\n        \"Efle Touwr\",\r\n        \"Linkden Parkington\",\r\n        \"There is no script engine for the file extension \\\".js\\\".\",\r\n        \"bob Rossington\",\r\n        \"Snaken Boot\",\r\n        \"Orbius the Rotund\",\r\n        \"llanfairpwllgwyngyllgogerychwyrndrobwllllantysiliogogogoch\",\r\n        \"Kenjamin Bindell\",\r\n        \"Sadney Krosby\",\r\n        \"Mevgeni Balkin\",\r\n        \"Kross LeTing\",\r\n        \"Name\",\r\n        \"M.A Flower\",\r\n        \"Bronald F TeddyBear\",\r\n        \"khill Pessel\",\r\n        \"Arty Sheeloovs\",\r\n        \"John \\\"Band of The Damned\\\" Damned\",\r\n        \"John GoblinBane\"\r\n    ]\r\n}", "0205cbcc699dce8e10a1b0d9bd9ba4f86ca7da5675af67057802dfd5c7aa932d");
            string decrypted = Bastion.PersistanceManager.Decrypt(encrypted, "0205cbcc699dce8e10a1b0d9bd9ba4f86ca7da5675af67057802dfd5c7aa932d");

            Debug.Log(encrypted + " | " +  decrypted);
        }

        public void PushNewData(string[] data, string path)
        {
            this.path = path;

            inputField.text = "";

            foreach (string s in data)
            {
                inputField.text += s + "\n";
            }

            UpdateUI();
        }

        public void UpdateUI()
        {
            string[] split = path.Replace("\\", "/").Split('/');

            fileName = split[split.Length - 1];

            nameBar.text = fileName;

            if (unsavedChanges)
                nameBar.text += "*";
        }

        public void PushChanges()
        {
            if (unsavedChanges == false)
            {
                unsavedChanges = true;
                UpdateUI();
            }

            em.SendDataToFV(inputField.text);
        }

        public void SaveCurrentData()
        {
            unsavedChanges = false;

            string[] lines = inputField.text.Split("\n");

            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (string s in lines)
                {
                    sw.WriteLine(s);
                }
            }

            UpdateUI();
        }
    }
}