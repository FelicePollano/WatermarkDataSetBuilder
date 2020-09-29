using System;
using System.Collections.Generic;

namespace WatermarkDataSetBuilder
{
    public class RandomCopyrightGenerator
    {
        Random random;
        public RandomCopyrightGenerator(int seed)
        {
            random = new Random(seed);
        }
        public string GetRandom()
        {
            List<string> parts = new List<string>();
            var r = random.Next(4);
            parts.Add(names[random.Next(names.Length)]);
            for(int i=0;i<r;++i)
            {
                parts.Add(lastnames[random.Next(lastnames.Length)]);
            }
            return string.Join(" ",parts);
        }

        string[] names = {
"autowns",
"ainsificansion",
"spourmo",
"deironts",
"melochection",
"veytoy",
"ratica",
"berictions",
"valize",
"gapatter",
"avesslational",
"minulle",
"pougglisomers",
"fantabulous",
"prograker",
"cistersion",
"craptacular",
"declain",
"advandant",
"prospeartented",
"shiphile",
"pendaytive",
"eakpoilets",
"saskimplaid",
"imperfied",
"fussople",
"stedientons",
"strastip",
"cracetchammiss",
"bellignorance",
"ductates",
"lusistrainest",
"undorsescites",
"hattitude",
"heassifilts",
"trantoe",
"doggax",
"builianis",
"morior",
"coloringlons",
"shothi",
"patrottenondal",
"compley",
"dreaker",
"connilletton",
"ailmenets",
"quident",
"aplayeard",
"reveforly",
"editactucar",


        };
        string[] lastnames={
"decrevendowney",
"aminter",
"dipolinestigns",
"dimens",
"aestons",
"eggmode",
"handian",
"arintingly",
"lidaneers",
"yostopholskia",
"micrafrify",
"vikins",
"lacircificane",
"exprecis",
"hypoplate",
"satinge",
"solibewither",
"imsynting",
"evenchically",
"nonsives",
"clambitablent",
"writivent",
"pluncidelst",
"tritatwates",
"adwadiansie",
"dinationereens",
"belleciarcle",
"excerow",
"fectors",
"covery",
"denonia",
"surelantered",
"antilds",
"opsablepsia",
"sistock",
"rabletuardines",
"aeroofile",
"expaing",
"dorchis",
"pendaytive",
"traffordon",
"stricarrorb",
"civided",
"doggax",
"clossyo",
"cizzbor",
"vallume",
"wilklablacquit",
"taindust",
"holyopt",
    };
    }

}