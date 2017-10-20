using System;

namespace App9
{
	static class Constants
	{
		internal static string strGoogleServerKey = "AIzaSyD6h_YtJnCIpD5v38RrU395mk_ZFvuGAgw";
		internal static string strGoogleServerDirKey= "AIzaSyD6h_YtJnCIpD5v38RrU395mk_ZFvuGAgw";
		internal static string strGoogleDirectionUrl="https://maps.googleapis.com/maps/api/directions/json?origin={0}&destination={1}&key="+strGoogleServerDirKey+"";
		internal static string strGeoCodingUrl="https://maps.googleapis.com/maps/api/geocode/json?{0}&key="+strGoogleServerKey+"";
	//	internal static string strSourceLocation="Vijayanagar,Bangalore,India";
	//	internal static string strDestinationLocation="Jayanagar,Bangalore,India";

		internal static string strException="Exception";
		internal static string strTextSource="Source";
		internal static string strTextDestination="Destination"; 

		internal static string strNoInternet="No online connection. Please review your internet connection"; 
		internal static string strPleaseWait="Please wait...";
		internal static string strUnableToConnect="Unable to connect server!,Please try after sometime"; 
	}

     class ListViewData
    {
        public string Id { get; set; }
        public string Name { get; set; }

    } 
}

