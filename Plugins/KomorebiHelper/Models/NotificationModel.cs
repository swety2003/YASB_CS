﻿//----------------------
// <auto-generated>
//     Generated using the NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0) (http://NJsonSchema.org)
// </auto-generated>
//----------------------

using System.Collections.Generic;


namespace KomorebiHelper.Models
{
    public class @event
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object content { get; set; }
    }

    public class Size
    {
        /// <summary>
        /// 
        /// </summary>
        public int left { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int top { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int right { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int bottom { get; set; }
    }

    public class Work_area_size
    {
        /// <summary>
        /// 
        /// </summary>
        public int left { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int top { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int right { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int bottom { get; set; }
    }

    public class Rect
    {
        /// <summary>
        /// 
        /// </summary>
        public int left { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int top { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int right { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int bottom { get; set; }
    }

    public class windows_item
    {
        /// <summary>
        /// 
        /// </summary>
        public int hwnd { get; set; }
        /// <summary>
        /// APP-CS (正在运行) - Microsoft Visual Studio Preview
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string exe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string @class { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Rect rect { get; set; }
    }

    public class Windows
    {
        /// <summary>
        /// 
        /// </summary>
        public List<windows_item> elements { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int focused { get; set; }
    }

    public class container_item
    {
        /// <summary>
        /// 
        /// </summary>
        public Windows windows { get; set; }
    }

    public class Containers
    {
        /// <summary>
        /// 
        /// </summary>
        public List<container_item> elements { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int focused { get; set; }
    }

    public class Layout
    {
        /// <summary>
        /// 
        /// </summary>
        public string @Default { get; set; }
    }
    public class workspace_item
    {
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Containers containers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string monocle_container { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string maximized_window { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> floating_windows { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Layout layout { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> layout_rules { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string layout_flip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int workspace_padding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int container_padding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<object> resize_dimensions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tile { get; set; }
    }

    public class Workspaces
    {
        /// <summary>
        /// 
        /// </summary>
        public List<workspace_item> elements { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int focused { get; set; }
    }

    public class monitor_item
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Size size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Work_area_size work_area_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string work_area_offset { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workspaces workspaces { get; set; }
    }

    public class Monitors
    {
        /// <summary>
        /// 
        /// </summary>
        public List<monitor_item> elements { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int focused { get; set; }
    }

    public class Invisible_borders
    {
        /// <summary>
        /// 
        /// </summary>
        public int left { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int top { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int right { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int bottom { get; set; }
    }

    public class Float_identifiersItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string kind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string matching_strategy { get; set; }
    }

    public class Manage_identifiersItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string kind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string matching_strategy { get; set; }
    }

    public class Layered_whitelistItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string kind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string matching_strategy { get; set; }
    }

    public class Tray_and_multi_window_identifiersItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string kind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string matching_strategy { get; set; }
    }

    public class Border_overflow_identifiersItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string kind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string matching_strategy { get; set; }
    }

    public class Name_change_on_launch_identifiersItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string kind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string matching_strategy { get; set; }
    }

    public class Monitor_index_preferences
    {
    }

    public class State
    {
        /// <summary>
        /// 
        /// </summary>
        public Monitors monitors { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_paused { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Invisible_borders invisible_borders { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int resize_delta { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string new_window_behaviour { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cross_monitor_move_behaviour { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string work_area_offset { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string focus_follows_mouse { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mouse_follows_focus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string has_pending_raise_op { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string remove_titlebars { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Float_identifiersItem> float_identifiers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Manage_identifiersItem> manage_identifiers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Layered_whitelistItem> layered_whitelist { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Tray_and_multi_window_identifiersItem> tray_and_multi_window_identifiers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Border_overflow_identifiersItem> border_overflow_identifiers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Name_change_on_launch_identifiersItem> name_change_on_launch_identifiers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Monitor_index_preferences monitor_index_preferences { get; set; }
    }

    public class JsonDataRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public @event @event { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public State state { get; set; }
    }

}