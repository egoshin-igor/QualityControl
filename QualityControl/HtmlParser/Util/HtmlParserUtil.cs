using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using HtmlParser.Dto;

namespace HtmlParser.Util
{
    internal static class HtmlParserUtil
    {
        const int BAD_REQUEST = 400;
        const string protocol = "http";

        public static LinksWrapper GetAllLinksInDomain( string urlAddress )
        {
            var normalLinks = new List<LinkResult>();
            var brokenLinks = new List<LinkResult>();
            var allStrLinks = GetAllStringLinksInDomain( urlAddress );
            foreach ( var link in allStrLinks )
            {
                var linkResult = new LinkResult() { Link = link, Status = GetStatusCode( link ) };
                var statusCode = linkResult.Status;
                if ( statusCode < BAD_REQUEST )
                {
                    normalLinks.Add( linkResult );
                }
                else
                {
                    brokenLinks.Add( linkResult );
                }
            }

            return new LinksWrapper
            {
                NormalLinks = normalLinks,
                BrokenLinks = brokenLinks
            };
        }

        private static List<string> GetAllStringLinksInDomain( string urlAddress )
        {
            var allLinks = new List<string>();
            var linksQueue = new Queue<string>();
            linksQueue.Enqueue( urlAddress );
            var newLinks = new List<string>();
            var parrentUri = new Uri( urlAddress );

            while ( linksQueue.Count != 0 )
            {
                var strLink = linksQueue.Dequeue();
                newLinks.Clear();
                try
                {
                    newLinks = GetLinks( parrentUri, strLink );
                }
                catch
                {
                    newLinks.Add( strLink );
                }

                foreach ( var link in newLinks )
                {
                    if ( !allLinks.Contains( link ) )
                    {
                        allLinks.Add( link );
                        linksQueue.Enqueue( link );
                    }
                }
            }

            return allLinks;
        }

        private static List<string> GetLinks( Uri parrentUri, string urlAddress )
        {
            var fixedLink = GetFixedLink( parrentUri, urlAddress );
            var linksList = new List<string>();
            if ( fixedLink == null )
            {
                return linksList;
            }

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml( new WebClient().DownloadString( urlAddress ) );

            var links = html.DocumentNode.SelectNodes( "//a[@href]" );
            foreach ( HtmlNode link in links )
            {
                HtmlAttribute att = link.Attributes[ "href" ];
                string strLink = att.Value;
                strLink = GetFixedLink( parrentUri, strLink );
                if ( strLink != null )
                {
                    linksList.Add( strLink );
                }
            }
            return linksList;
        }


        private static int GetStatusCode( string urlAddress )
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = ( HttpWebRequest )WebRequest.Create( urlAddress );
                request.Method = "HEAD";
                response = ( HttpWebResponse )request.GetResponse();
            }
            catch ( WebException we )
            {
                response = ( HttpWebResponse )we.Response;
            }

            if ( response != null )
            {
                return ( int )response.StatusCode;
            }

            return 500;
        }

        private static string GetFixedLink( Uri parrentUri, string urlAdress )
        {
            var absoluteUri = parrentUri.AbsoluteUri;
            var path = parrentUri.AbsolutePath;
            var domainAddress = new Uri( ( absoluteUri.Remove( absoluteUri.Length - path.Length ) ) );
            if ( urlAdress.Length <= 1 )
            {
                return null;
            }

            if ( urlAdress[ 0 ] == '/' )
            {
                if ( urlAdress[ 1 ] == '/' )
                {
                    return parrentUri.Scheme + ":" + urlAdress;
                }

                return new Uri( domainAddress, urlAdress ).AbsoluteUri;
            }

            if ( urlAdress.Contains( parrentUri.Authority ) ||
                 !urlAdress.Contains( protocol ) &&
                 !urlAdress.Contains( "javascript" ) &&
                 !urlAdress.Contains( "mailto:" ) &&
                 !urlAdress.Contains( "tel:" ) )
            {
                return new Uri( domainAddress, urlAdress ).AbsoluteUri;
            }

            return null;
        }
    }
}
