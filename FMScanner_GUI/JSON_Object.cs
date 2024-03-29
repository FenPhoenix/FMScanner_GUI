﻿using System;

namespace FMScanner_GUI
{
    public class TTLG_Thread
    {
        public long id = 0;
        public string name = "";
    }

    public class NewDark
    {
        public bool is_required = false;
        public string minimum_required_version = "";
    }

    public class Characteristics
    {
        public bool has_custom_scripts = false;
        public bool has_custom_textures = false;
        public bool has_custom_sounds = false;
        public bool has_custom_objects = false;
        public bool has_custom_creatures = false;
        public bool has_custom_motions = false;
        public bool has_custom_subtitles = false;
        public bool has_automap = false;
        public bool has_movies = false;
        public bool has_map = false;
    }

    public class Details
    {
        public string game = "";
        public TTLG_Thread ttlg_thread = new();
        public string[] categories = { "" };
        public NameAndUrl contest = new();
        public string[] languages = { "" };
        public string version = "";
        public NewDark newdark = new();
        public string original_release_date = "";
        public string last_update_date = "";
        public Characteristics characteristics = new();
    }

    public class Extras
    {
        public Download download = new();
        public Screenshots screenshots = new();
        public YouTube youtube = new();
        public NameAndUrl[] reviews = { new NameAndUrl() };
        public NameAndUrl[] walkthroughs = { new NameAndUrl() };
        public NameAndUrl[] loot_lists = { new NameAndUrl() };
        public NameAndUrl[] hints = { new NameAndUrl() };
    }

    public class Download
    {
        public DownloadMain main = new();
        public DownloadAlternative alternative = new();
    }

    public class DownloadMain
    {
        public Hoster hoster = new();
        public Reference[] references = { new Reference() };
    }

    public class DownloadAlternative
    {
        public Hoster hoster = new();
        public Reference[] references = { new Reference() };
    }

    public class Hoster
    {
        public string name = "";
        public string url = "";
    }

    public class Reference
    {
        public string language = "";
        public ulong size = 0;
        public string url = "";
    }

    public class Screenshots
    {
        public NameAndUrl source = new();
        public string[] references = { "" };
    }

    public class YouTube
    {
        public string id = "";
        public string name = "";
        public NameAndUrl channel = new();
    }

    public class NameAndUrl
    {
        public string name = "";
        public string url = "";
    }

    public class Series
    {
        public string name = "";
        public int part = 0;
        public int total_missions = 0;
    }

    public class JSON_Object
    {
        public bool draft = true;
        public string name = "";
        public string author = "";
        public string type = "";
        public Series series = new();
        //public int included_missions = 0;
        public Details details = new();
        public Extras extras = new();
    }
}
