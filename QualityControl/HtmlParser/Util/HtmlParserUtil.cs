using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace HtmlParser.Util
{
    internal static class HtmlParserUtil
    {
        public static List<string> GetLinksList( string urlAddress )
        {

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml( new WebClient().DownloadString( urlAddress ) );

            var parrentUri = new Uri( urlAddress );
            var hrefTags = new List<string>();
            foreach ( HtmlNode link in html.DocumentNode.SelectNodes( "//a[@href]" ) )
            {
                HtmlAttribute att = link.Attributes[ "href" ];
                string urlAdress = att.Value;
                urlAdress = GetFixedLink( parrentUri, urlAdress );
                if ( urlAdress != null )
                {
                    hrefTags.Add( urlAdress );
                }
            }

            return hrefTags;
        }

        private static string GetFixedLink( Uri parrentUri, string urlAdress )
        {
            var absoluteUri = parrentUri.AbsoluteUri;
            var path = parrentUri.AbsolutePath;
            var domainAddress = new Uri( ( absoluteUri.Remove( absoluteUri.Length - path.Length ) ) );

            if ( urlAdress.Length < 1 )
            {
                return null;
            }

            var parrentUriAddress = parrentUri.AbsoluteUri;
            if ( urlAdress[ 0 ] == '#' )
            {
                return parrentUri.AbsoluteUri + urlAdress;
            }

            if ( urlAdress[ 0 ] == '/' )
            {
                if ( urlAdress.IndexOf( parrentUri.Authority ) != -1 )
                {
                    return parrentUri.Scheme + ":" + urlAdress;
                }

                return new Uri( domainAddress, urlAdress ).AbsoluteUri;
            }

            if ( urlAdress.IndexOf( parrentUri.Authority ) != -1 )
            {
                return new Uri( domainAddress, urlAdress ).AbsoluteUri;
            }

            return null;
        }
    }
}
