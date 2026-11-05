using System.Collections.Generic;
using System.Collections.Frozen;
using Avalonia.Media;
using AppUI.Core.Icons;

namespace AppUI.Core.Icons;

public static class LucideIconProvider
{
    private static readonly FrozenDictionary<LucideIconKind, string> PathData = new Dictionary<LucideIconKind, string>
    {
        [LucideIconKind.ArrowDown] = "M 12 5 v 14 M 19 12 l -7 7 -7 -7",
        [LucideIconKind.ArrowLeft] = "M 12 19 l -7 -7 7 -7 M 19 12 H 5",
        [LucideIconKind.ArrowRight] = "M 5 12 h 14 M 12 5 l 7 7 -7 7",
        [LucideIconKind.ArrowUp] = "M 5 12 l 7 -7 7 7 M 12 19 V 5",
        [LucideIconKind.ArrowUpDown] = "M 21 16 l -4 4 -4 -4 M 17 20 V 4 M 3 8 l 4 -4 4 4 M 7 4 v 16",
        [LucideIconKind.Bell] = "M 10.268 21 a 2 2 0 0 0 3.464 0 M 3.262 15.326 A 1 1 0 0 0 4 17 h 16 a 1 1 0 0 0 .74 -1.673 C 19.41 13.956 18 12.499 18 8 A 6 6 0 0 0 6 8 c 0 4.499 -1.411 5.956 -2.738 7.326",
        [LucideIconKind.Briefcase] = "M 16 20 V 4 a 2 2 0 0 0 -2 -2 h -4 a 2 2 0 0 0 -2 2 v 16 M 4 6 h 16 a 2 2 0 0 1 2 2 v 10 a 2 2 0 0 1 -2 2 H 4 a 2 2 0 0 1 -2 -2 V 8 a 2 2 0 0 1 2 -2 Z",
        [LucideIconKind.Calendar] = "M 8 2 v 3 M 16 2 v 3 M 5 3 h 14 a 2 2 0 0 1 2 2 v 14 a 2 2 0 0 1 -2 2 H 5 a 2 2 0 0 1 -2 -2 V 5 a 2 2 0 0 1 2 -2 Z M 3 9 h 18",
        [LucideIconKind.Camera] = "M 13.997 4 a 2 2 0 0 1 1.76 1.05 l .486 .9 A 2 2 0 0 0 18.003 7 H 20 a 2 2 0 0 1 2 2 v 9 a 2 2 0 0 1 -2 2 H 4 a 2 2 0 0 1 -2 -2 V 9 a 2 2 0 0 1 2 -2 h 1.997 a 2 2 0 0 0 1.759 -1.048 l .489 -.904 A 2 2 0 0 1 10.004 4 z M 9 13 a 3 3 0 1 0 6 0 a 3 3 0 1 0 -6 0",
        [LucideIconKind.ChartColumn] = "M 3 3 v 16 a 2 2 0 0 0 2 2 h 16 M 18 17 V 9 M 13 17 V 5 M 8 17 v -3",
        [LucideIconKind.ChartLine] = "M 3 3 v 16 a 2 2 0 0 0 2 2 h 16 M 19 9 l -5 5 -4 -4 -3 3",
        [LucideIconKind.ChartPie] = "M 21 12 c .552 0 1.005 -.449 .95 -.998 a 10 10 0 0 0 -8.953 -8.951 c -.55 -.055 -.998 .398 -.998 .95 v 8 a 1 1 0 0 0 1 1 z M 21.21 15.89 A 10 10 0 1 1 8 2.83",
        [LucideIconKind.Check] = "M 20 6 L 9 17 l -5 -5",
        [LucideIconKind.CheckCheck] = "M 18 6 L 7 17 l -5 -5 M 22 10 l -7.5 7.5 L 13 16",
        [LucideIconKind.ChevronDown] = "M 6 9 l 6 6 6 -6",
        [LucideIconKind.ChevronLeft] = "M 15 18 l -6 -6 6 -6",
        [LucideIconKind.ChevronRight] = "M 9 18 l 6 -6 -6 -6",
        [LucideIconKind.ChevronUp] = "M 18 15 l -6 -6 -6 6",
        [LucideIconKind.Circle] = "M 2 12 a 10 10 0 1 0 20 0 a 10 10 0 1 0 -20 0",
        [LucideIconKind.CircleAlert] = "M 2 12 a 10 10 0 1 0 20 0 a 10 10 0 1 0 -20 0 M 12 8 v 4 M 12 16 h .01",
        [LucideIconKind.CircleCheck] = "M 2 12 a 10 10 0 1 0 20 0 a 10 10 0 1 0 -20 0 M 9 12 l 2 2 4 -4",
        [LucideIconKind.Clock] = "M 2 12 a 10 10 0 1 0 20 0 a 10 10 0 1 0 -20 0 M 12 6 v 6 l 4 2",
        [LucideIconKind.Code] = "M 16 18 l 6 -6 -6 -6 M 8 6 l -6 6 6 6",
        [LucideIconKind.Copy] = "M 10 8 h 10 a 2 2 0 0 1 2 2 v 10 a 2 2 0 0 1 -2 2 H 10 a 2 2 0 0 1 -2 -2 V 10 a 2 2 0 0 1 2 -2 Z M 4 16 c -1.1 0 -2 -.9 -2 -2 V 4 c 0 -1.1 .9 -2 2 -2 h 10 c 1.1 0 2 .9 2 2",
        [LucideIconKind.DollarSign] = "M 12 2 v 20 M 17 5 H 9.5 a 3.5 3.5 0 0 0 0 7 h 5 a 3.5 3.5 0 0 1 0 7 H 6",
        [LucideIconKind.Download] = "M 21 15 v 4 a 2 2 0 0 1 -2 2 H 5 a 2 2 0 0 1 -2 -2 v -4 M 7 10 l 5 5 5 -5 M 12 15 V 3",
        [LucideIconKind.EllipsisVertical] = "M 11 12 a 1 1 0 1 0 2 0 a 1 1 0 1 0 -2 0 M 11 5 a 1 1 0 1 0 2 0 a 1 1 0 1 0 -2 0 M 11 19 a 1 1 0 1 0 2 0 a 1 1 0 1 0 -2 0",
        [LucideIconKind.Eye] = "M 2.062 12.348 a 1 1 0 0 1 0 -.696 a 10.75 10.75 0 0 1 19.876 0 a 1 1 0 0 1 0 .696 a 10.75 10.75 0 0 1 -19.876 0 M 9 12 a 3 3 0 1 0 6 0 a 3 3 0 1 0 -6 0",
        [LucideIconKind.EyeOff] = "M 10.733 5.076 a 10.744 10.744 0 0 1 11.205 6.575 a 1 1 0 0 1 0 .696 a 10.747 10.747 0 0 1 -1.444 2.49 M 14.084 14.158 a 3 3 0 0 1 -4.242 -4.242 M 17.479 17.499 a 10.75 10.75 0 0 1 -15.417 -5.151 a 1 1 0 0 1 0 -.696 a 10.75 10.75 0 0 1 4.446 -5.143 M 2 2 L 22 22",
        [LucideIconKind.File] = "M 6 22 a 2 2 0 0 1 -2 -2 V 4 a 2 2 0 0 1 2 -2 h 8 a 2.4 2.4 0 0 1 1.704 .706 l 3.588 3.588 A 2.4 2.4 0 0 1 20 8 v 12 a 2 2 0 0 1 -2 2 Z M 14 2 v 5 a 1 1 0 0 0 1 1 h 5",
        [LucideIconKind.FileCheck] = "M 6 22 a 2 2 0 0 1 -2 -2 V 4 a 2 2 0 0 1 2 -2 h 8 a 2.4 2.4 0 0 1 1.704 .706 l 3.588 3.588 A 2.4 2.4 0 0 1 20 8 v 12 a 2 2 0 0 1 -2 2 Z M 14 2 v 5 a 1 1 0 0 0 1 1 h 5 M 9 15 l 2 2 4 -4",
        [LucideIconKind.FilePlus] = "M 6 22 a 2 2 0 0 1 -2 -2 V 4 a 2 2 0 0 1 2 -2 h 8 a 2.4 2.4 0 0 1 1.704 .706 l 3.588 3.588 A 2.4 2.4 0 0 1 20 8 v 12 a 2 2 0 0 1 -2 2 Z M 14 2 v 5 a 1 1 0 0 0 1 1 h 5 M 9 15 h 6 M 12 18 v -6",
        [LucideIconKind.FileText] = "M 6 22 a 2 2 0 0 1 -2 -2 V 4 a 2 2 0 0 1 2 -2 h 8 a 2.4 2.4 0 0 1 1.704 .706 l 3.588 3.588 A 2.4 2.4 0 0 1 20 8 v 12 a 2 2 0 0 1 -2 2 Z M 14 2 v 5 a 1 1 0 0 0 1 1 h 5 M 10 9 H 8 M 16 13 H 8 M 16 17 H 8",
        [LucideIconKind.Files] = "M 15 2 H 11 a 2 2 0 0 0 -2 2 v 11 a 2 2 0 0 0 2 2 h 8 a 2 2 0 0 0 2 -2 V 8 M 16.706 2.706 A 2.4 2.4 0 0 0 15 2 v 5 a 1 1 0 0 0 1 1 h 5 a 2.4 2.4 0 0 0 -.706 -1.706 Z M 5 7 a 2 2 0 0 0 -2 2 v 11 a 2 2 0 0 0 2 2 h 8 a 2 2 0 0 0 1.732 -1",
        [LucideIconKind.Filter] = "M 10 20 a 1 1 0 0 0 .553 .895 l 2 1 A 1 1 0 0 0 14 21 v -7 a 2 2 0 0 1 .517 -1.341 L 21.74 4.67 A 1 1 0 0 0 21 3 H 3 a 1 1 0 0 0 -.742 1.67 l 7.225 7.989 A 2 2 0 0 1 10 14 Z",
        [LucideIconKind.Folder] = "M 4 20 h 16 a 2 2 0 0 0 2 -2 V 8 a 2 2 0 0 0 -2 -2 h -7.9 a 2 2 0 0 1 -1.69 -.9 l -.81 -1.2 A 2 2 0 0 0 7.9 3 H 4 a 2 2 0 0 0 -2 2 v 13 a 2 2 0 0 0 2 2 Z",
        [LucideIconKind.FolderOpen] = "M 6 14 l 1.5 -2.9 A 2 2 0 0 1 9.24 10 H 20 a 2 2 0 0 1 1.94 2.5 l -1.54 6 a 2 2 0 0 1 -1.95 1.5 H 4 a 2 2 0 0 1 -2 -2 V 5 a 2 2 0 0 1 2 -2 h 3.9 a 2 2 0 0 1 1.69 .9 l .81 1.2 a 2 2 0 0 0 1.67 .9 H 18 a 2 2 0 0 1 2 2 v 2",
        [LucideIconKind.House] = "M 15 21 v -8 a 1 1 0 0 0 -1 -1 h -4 a 1 1 0 0 0 -1 1 v 8 M 3 10 a 2 2 0 0 1 .709 -1.528 l 7 -6 a 2 2 0 0 1 2.582 0 l 7 6 A 2 2 0 0 1 21 10 v 9 a 2 2 0 0 1 -2 2 H 5 a 2 2 0 0 1 -2 -2 Z",
        [LucideIconKind.Image] = "M 5 3 h 14 a 2 2 0 0 1 2 2 v 14 a 2 2 0 0 1 -2 2 H 5 a 2 2 0 0 1 -2 -2 V 5 a 2 2 0 0 1 2 -2 Z M 7 9 a 2 2 0 1 0 4 0 a 2 2 0 1 0 -4 0 M 21 15 l -3.086 -3.086 a 2 2 0 0 0 -2.828 0 L 6 21",
        [LucideIconKind.Info] = "M 2 12 a 10 10 0 1 0 20 0 a 10 10 0 1 0 -20 0 M 12 16 v -4 M 12 8 h .01",
        [LucideIconKind.Key] = "M 2 21 l 9.6 -9.6 M 19 8 l 2.3 2.3 a 1 1 0 0 1 0 1.4 l -2.1 2.1 a 1 1 0 0 1 -1.4 0 L 15.5 11.5 M 10 7.5 a 5.5 5.5 0 1 0 11 0 a 5.5 5.5 0 1 0 -11 0",
        [LucideIconKind.LayoutGrid] = "M 4 3 h 5 a 1 1 0 0 1 1 1 v 5 a 1 1 0 0 1 -1 1 H 4 a 1 1 0 0 1 -1 -1 V 4 a 1 1 0 0 1 1 -1 Z M 15 3 h 5 a 1 1 0 0 1 1 1 v 5 a 1 1 0 0 1 -1 1 h -5 a 1 1 0 0 1 -1 -1 V 4 a 1 1 0 0 1 1 -1 Z M 15 14 h 5 a 1 1 0 0 1 1 1 v 5 a 1 1 0 0 1 -1 1 h -5 a 1 1 0 0 1 -1 -1 v -5 a 1 1 0 0 1 1 -1 Z M 4 14 h 5 a 1 1 0 0 1 1 1 v 5 a 1 1 0 0 1 -1 1 H 4 a 1 1 0 0 1 -1 -1 v -5 a 1 1 0 0 1 1 -1 Z",
        [LucideIconKind.List] = "M 3 5 h .01 M 3 12 h .01 M 3 19 h .01 M 8 5 h 13 M 8 12 h 13 M 8 19 h 13",
        [LucideIconKind.Lock] = "M 5 11 h 14 a 2 2 0 0 1 2 2 v 7 a 2 2 0 0 1 -2 2 H 5 a 2 2 0 0 1 -2 -2 v -7 a 2 2 0 0 1 2 -2 Z M 7 11 V 7 a 5 5 0 0 1 10 0 v 4",
        [LucideIconKind.LogOut] = "M 16 17 l 5 -5 -5 -5 M 21 12 H 9 M 9 21 H 5 a 2 2 0 0 1 -2 -2 V 5 a 2 2 0 0 1 2 -2 h 4",
        [LucideIconKind.Mail] = "M 22 7 l -8.991 5.727 a 2 2 0 0 1 -2.009 0 L 2 7 M 4 4 h 16 a 2 2 0 0 1 2 2 v 12 a 2 2 0 0 1 -2 2 H 4 a 2 2 0 0 1 -2 -2 V 6 a 2 2 0 0 1 2 -2 Z",
        [LucideIconKind.MailOpen] = "M 21.2 8.4 c .5 .38 .8 .97 .8 1.6 v 10 a 2 2 0 0 1 -2 2 H 4 a 2 2 0 0 1 -2 -2 V 10 a 2 2 0 0 1 .8 -1.6 l 8 -6 a 2 2 0 0 1 2.4 0 l 8 6 Z M 22 10 l -8.97 5.7 a 1.94 1.94 0 0 1 -2.06 0 L 2 10",
        [LucideIconKind.Menu] = "M 4 5 h 16 M 4 12 h 16 M 4 19 h 16",
        [LucideIconKind.Minus] = "M 5 12 h 14",
        [LucideIconKind.Moon] = "M 20.985 12.486 a 9 9 0 1 1 -9.473 -9.472 c .405 -.022 .617 .46 .402 .803 a 6 6 0 0 0 8.268 8.268 c .344 -.215 .825 -.004 .803 .401",
        [LucideIconKind.Package] = "M 11 21.73 a 2 2 0 0 0 2 0 l 7 -4 A 2 2 0 0 0 21 16 V 8 a 2 2 0 0 0 -1 -1.73 l -7 -4 a 2 2 0 0 0 -2 0 l -7 4 A 2 2 0 0 0 3 8 v 8 a 2 2 0 0 0 1 1.73 Z M 12 22 V 12 M 3.29 7 L 12 12 L 20.71 7 M 16.5 9.4 L 7.55 4.24",
        [LucideIconKind.PackageOpen] = "M 12 22 v -9 M 15.17 2.21 a 1.67 1.67 0 0 1 1.63 0 L 21 4.57 a 1.93 1.93 0 0 1 0 3.36 L 8.82 14.79 a 1.655 1.655 0 0 1 -1.64 0 L 3 12.43 a 1.93 1.93 0 0 1 0 -3.36 Z M 20 13 v 3.87 a 2.06 2.06 0 0 1 -1.11 1.83 l -6 3.08 a 1.93 1.93 0 0 1 -1.78 0 l -6 -3.08 A 2.06 2.06 0 0 1 4 16.87 V 13 M 21 12.43 a 1.93 1.93 0 0 0 0 -3.36 L 8.83 2.2 a 1.64 1.64 0 0 0 -1.63 0 L 3 4.57 a 1.93 1.93 0 0 0 0 3.36 l 12.18 6.86 a 1.636 1.636 0 0 0 1.63 0 Z",
        [LucideIconKind.Pencil] = "M 21.174 6.812 a 1 1 0 0 0 -3.986 -3.987 L 3.842 16.174 a 2 2 0 0 0 -.5 .83 l -1.321 4.352 a .5 .5 0 0 0 .623 .622 l 4.353 -1.32 a 2 2 0 0 0 .83 -.497 Z M 15 5 l 4 4",
        [LucideIconKind.Plus] = "M 5 12 h 14 M 12 5 v 14",
        [LucideIconKind.Printer] = "M 6 18 H 4 a 2 2 0 0 1 -2 -2 v -5 a 2 2 0 0 1 2 -2 h 16 a 2 2 0 0 1 2 2 v 5 a 2 2 0 0 1 -2 2 h -2 M 6 9 V 3 a 1 1 0 0 1 1 -1 h 10 a 1 1 0 0 1 1 1 v 6 M 7 14 h 10 a 1 1 0 0 1 1 1 v 6 a 1 1 0 0 1 -1 1 H 7 a 1 1 0 0 1 -1 -1 v -6 a 1 1 0 0 1 1 -1 Z",
        [LucideIconKind.QrCode] = "M 4 3 h 3 a 1 1 0 0 1 1 1 v 3 a 1 1 0 0 1 -1 1 H 4 a 1 1 0 0 1 -1 -1 V 4 a 1 1 0 0 1 1 -1 Z M 17 3 h 3 a 1 1 0 0 1 1 1 v 3 a 1 1 0 0 1 -1 1 h -3 a 1 1 0 0 1 -1 -1 V 4 a 1 1 0 0 1 1 -1 Z M 4 16 h 3 a 1 1 0 0 1 1 1 v 3 a 1 1 0 0 1 -1 1 H 4 a 1 1 0 0 1 -1 -1 v -3 a 1 1 0 0 1 1 -1 Z M 21 16 h -3 a 2 2 0 0 0 -2 2 v 3 M 21 21 v .01 M 12 7 v 3 a 2 2 0 0 1 -2 2 H 7 M 3 12 h .01 M 12 3 h .01 M 12 16 v .01 M 16 12 h 1 M 21 12 v .01 M 12 21 v -1",
        [LucideIconKind.RefreshCw] = "M 3 12 a 9 9 0 0 1 9 -9 a 9.75 9.75 0 0 1 6.74 2.74 L 21 8 M 21 3 v 5 h -5 M 21 12 a 9 9 0 0 1 -9 9 a 9.75 9.75 0 0 1 -6.74 -2.74 L 3 16 M 8 16 H 3 v 5",
        [LucideIconKind.RotateCcw] = "M 3 12 a 9 9 0 1 0 9 -9 a 9.75 9.75 0 0 0 -6.74 2.74 L 3 8 M 3 3 v 5 h 5",
        [LucideIconKind.Search] = "M 21 21 l -4.34 -4.34 M 3 11 a 8 8 0 1 0 16 0 a 8 8 0 1 0 -16 0",
        [LucideIconKind.Send] = "M 14.536 21.686 a .5 .5 0 0 0 .937 -.024 l 6.5 -19 a .496 .496 0 0 0 -.635 -.635 l -19 6.5 a .5 .5 0 0 0 -.024 .937 l 7.93 3.18 a 2 2 0 0 1 1.112 1.11 Z M 22 2 L 11 13",
        [LucideIconKind.Settings] = "M 9.671 4.136 a 2.34 2.34 0 0 1 4.659 0 a 2.34 2.34 0 0 0 3.319 1.915 a 2.34 2.34 0 0 1 2.33 4.033 a 2.34 2.34 0 0 0 0 3.831 a 2.34 2.34 0 0 1 -2.33 4.033 a 2.34 2.34 0 0 0 -3.319 1.915 a 2.34 2.34 0 0 1 -4.659 0 a 2.34 2.34 0 0 0 -3.32 -1.915 a 2.34 2.34 0 0 1 -2.33 -4.033 a 2.34 2.34 0 0 0 0 -3.831 A 2.34 2.34 0 0 1 6.35 6.051 a 2.34 2.34 0 0 0 3.319 -1.915 M 9 12 a 3 3 0 1 0 6 0 a 3 3 0 1 0 -6 0",
        [LucideIconKind.Shield] = "M 20 13 c 0 5 -3.5 7.5 -7.66 8.95 a 1 1 0 0 1 -.67 -.01 C 7.5 20.5 4 18 4 13 V 6 a 1 1 0 0 1 1 -1 c 2 0 4.5 -1.2 6.24 -2.72 a 1.17 1.17 0 0 1 1.52 0 C 14.51 3.81 17 5 19 5 a 1 1 0 0 1 1 1 Z",
        [LucideIconKind.ShoppingBag] = "M 16 10 a 4 4 0 0 1 -8 0 M 3.103 6.034 h 17.794 M 3.4 5.467 a 2 2 0 0 0 -.4 1.2 V 20 a 2 2 0 0 0 2 2 h 14 a 2 2 0 0 0 2 -2 V 6.667 a 2 2 0 0 0 -.4 -1.2 l -2 -2.667 A 2 2 0 0 0 17 2 H 7 a 2 2 0 0 0 -1.6 .8 Z",
        [LucideIconKind.ShoppingCart] = "M 2.05 2.05 l 1.099 -.028 a 1 1 0 0 1 1.008 .815 l 2.69 14.347 A 1 1 0 0 0 7.83 18 H 18 M 4.563 5 h 16.435 a 1 1 0 0 1 .981 1.204 l -1.026 6.226 A 2 2 0 0 1 18.962 14 H 6.25 M 16 20 a 2 2 0 1 0 4 0 a 2 2 0 1 0 -4 0 M 6 20 a 2 2 0 1 0 4 0 a 2 2 0 1 0 -4 0",
        [LucideIconKind.Smartphone] = "M 7 2 h 10 a 2 2 0 0 1 2 2 v 16 a 2 2 0 0 1 -2 2 H 7 a 2 2 0 0 1 -2 -2 V 4 a 2 2 0 0 1 2 -2 Z M 12 18 h .01",
        [LucideIconKind.Square] = "M 5 3 h 14 a 2 2 0 0 1 2 2 v 14 a 2 2 0 0 1 -2 2 H 5 a 2 2 0 0 1 -2 -2 V 5 a 2 2 0 0 1 2 -2 Z",
        [LucideIconKind.Star] = "M 11.525 2.295 a .53 .53 0 0 1 .95 0 l 2.31 4.679 a 2.123 2.123 0 0 0 1.595 1.16 l 5.166 .756 a .53 .53 0 0 1 .294 .904 l -3.736 3.638 a 2.123 2.123 0 0 0 -.611 1.878 l .882 5.14 a .53 .53 0 0 1 -.771 .56 l -4.618 -2.428 a 2.122 2.122 0 0 0 -1.973 0 L 6.396 21.01 a .53 .53 0 0 1 -.77 -.56 l .881 -5.139 a 2.122 2.122 0 0 0 -.611 -1.879 L 2.16 9.795 a .53 .53 0 0 1 .294 -.906 l 5.165 -.755 a 2.122 2.122 0 0 0 1.597 -1.16 Z",
        [LucideIconKind.Sun] = "M 8 12 a 4 4 0 1 0 8 0 a 4 4 0 1 0 -8 0 M 12 2 v 2 M 12 20 v 2 M 4.93 4.93 l 1.41 1.41 M 17.66 17.66 l 1.41 1.41 M 2 12 h 2 M 20 12 h 2 M 6.34 17.66 l -1.41 1.41 M 19.07 4.93 l -1.41 1.41",
        [LucideIconKind.SunMoon] = "M 12 2 v 2 M 14.837 16.385 a 6 6 0 1 1 -7.223 -7.222 c .624 -.147 .97 .66 .715 1.248 a 4 4 0 0 0 5.26 5.259 c .589 -.255 1.396 .09 1.248 .715 M 16 12 a 4 4 0 0 0 -4 -4 M 19 5 l -1.256 1.256 M 20 12 h 2",
        [LucideIconKind.Trash2] = "M 10 11 v 6 M 14 11 v 6 M 19 6 v 14 a 2 2 0 0 1 -2 2 H 7 a 2 2 0 0 1 -2 -2 V 6 M 3 6 h 18 M 8 6 V 4 a 2 2 0 0 1 2 -2 h 4 a 2 2 0 0 1 2 2 v 2",
        [LucideIconKind.TriangleAlert] = "M 21.73 18 l -8 -14 a 2 2 0 0 0 -3.48 0 l -8 14 A 2 2 0 0 0 4 21 h 16 a 2 2 0 0 0 1.73 -3 M 12 9 v 4 M 12 17 h .01",
        [LucideIconKind.Upload] = "M 12 3 v 12 M 17 8 L 12 3 L 7 8 M 21 15 v 4 a 2 2 0 0 1 -2 2 H 5 a 2 2 0 0 1 -2 -2 v -4",
        [LucideIconKind.User] = "M 19 21 v -2 a 4 4 0 0 0 -4 -4 H 9 a 4 4 0 0 0 -4 4 v 2 M 8 7 a 4 4 0 1 0 8 0 a 4 4 0 1 0 -8 0",
        [LucideIconKind.UserPlus] = "M 16 21 v -2 a 4 4 0 0 0 -4 -4 H 6 a 4 4 0 0 0 -4 4 v 2 M 5 7 a 4 4 0 1 0 8 0 a 4 4 0 1 0 -8 0 M 19 8 v 6 M 22 11 h -6",
        [LucideIconKind.Users] = "M 16 21 v -2 a 4 4 0 0 0 -4 -4 H 6 a 4 4 0 0 0 -4 4 v 2 M 16 3.128 a 4 4 0 0 1 0 7.744 M 22 21 v -2 a 4 4 0 0 0 -3 -3.87 M 5 7 a 4 4 0 1 0 8 0 a 4 4 0 1 0 -8 0",
        [LucideIconKind.X] = "M 18 6 L 6 18 M 6 6 L 18 18",
        [LucideIconKind.Zap] = "M 15.914 4 a 1.5 1.5 0 0 0 -2.474 -1.561 l -9 9 A 1.5 1.5 0 0 0 5.5 14 h 4.002 a .5 .5 0 0 1 .471 .666 L 8.086 20 a 1.5 1.5 0 0 0 2.475 1.56 l 9 -9 A 1.5 1.5 0 0 0 18.5 10 h -3.997 a .5 .5 0 0 1 -.472 -.667 Z",
    }.ToFrozenDictionary();

    private static readonly System.Collections.Concurrent.ConcurrentDictionary<LucideIconKind, StreamGeometry> CachedGeometries = new();

    public static StreamGeometry GetGeometry(LucideIconKind kind)
    {
        return CachedGeometries.GetOrAdd(kind, static k =>
        {
            if (PathData.TryGetValue(k, out var data))
            {
                return StreamGeometry.Parse(data);
            }
            return new StreamGeometry();
        });
    }
}
