using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace vnaisoft.common.Helpers

{
    public static class SoapExtensions
    {
        public static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        public static XmlDocument CreateSoapEnvelope()
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(
            @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body>
                    <publishInvWithToken xmlns = ""http://tempuri.org/"">
                      <Account>1200224584_admin_demo</Account>
                      <ACpass>123456aA@</ACpass>
                      <xmlInvData><![CDATA[<Invoices><Inv><key></key><Invoice><Buyer>Suporrt VNPT - IT</Buyer><CusCode>01500118</CusCode><CusName>Suporrt VNPT - IT</CusName><CusAddress>57 Huỳnh Thúc Kháng Hà Nội</CusAddress><CusPhone></CusPhone><CusTaxCode></CusTaxCode><PaymentMethod>TM</PaymentMethod><KindOfService>3/2021</KindOfService><EmailDeliver>kinhdoanhcn1@apic.com.vn</EmailDeliver><Products><Product><ProdName>Sản phẩm </ProdName><ProdUnit>kg</ProdUnit><ProdQuantity>562.9000</ProdQuantity><ProdPrice>8400</ProdPrice><VATRate>10</VATRate><Amount>4728360</Amount></Product></Products><VATRate>10</VATRate><VATAmount>472836</VATAmount><Total>4728360</Total><Amount>5201196</Amount><AmountInWords>Năm triệu </AmountInWords><ArisingDate>15/03/2021</ArisingDate></Invoice></Inv></Invoices>]]></xmlInvData>
                      <username>1200224584_service</username>
                      <password>123456aA@</password>
                      <pattern>02GTTT0/001</pattern>
                      <serial>KD/20E</serial>
                    </publishInvWithToken>
                </soap:Body>
            </soap:Envelope>");
            return soapEnvelopeDocument;
        }

        public static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        //public static XmlDocument CreateSoapEnvelopeGetHashCTTWithToken(string adminAccount,
        //    string adminPass,
        //    string xml,
        //    string wsAccount,
        //    string wsPass,
        //    string serialCert,
        //    string pattern,
        //    string serial)
        //{
        //    XmlDocument document = new XmlDocument();
        //    string xmlString = $"<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
        //        $"<soap:Body>" +
        //        $"<getHashCTTWithToken xmlns=\"http://tempuri.org/\">" +
        //        $"<Account>{adminAccount}</Account>" +
        //        $"<ACpass>{adminPass}</ACpass>" +
        //        $"<xmlInvData><![CDATA[{xml}]]></xmlInvData>" +
        //        $"<username>{wsAccount}</username>" +
        //        $"<password>{wsPass}</password>" +
        //        $"<serialCert>{serialCert}</serialCert>" +
        //        $"<type>0</type>" +
        //        $"<invToken></invToken>" +
        //        $"<pattern>{pattern}</pattern>" +
        //        $"<serial>{serial}</serial>" +
        //        $"<convert>0</convert>" +
        //        $"</getHashCTTWithToken>" +
        //        $"</soap:Body>" +
        //        $"</soap:Envelope>";

        //    document.LoadXml(xmlString);
        //    return document;
        //}

        
            public static XmlDocument ReplaceInvoiceAction(string id, string adminAccount,
            string adminPass,
            string xml,
            string wsAccount,
            string wsPass,
            string pattern,
            string serial)
        {
            XmlDocument document = new XmlDocument();
            string xmlString = $"<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                $"<soap:Body>" +
                $"<ReplaceInvoiceAction xmlns=\"http://tempuri.org/\">" +
                $"<Account>{adminAccount}</Account>" +
                $"<ACpass>{adminPass}</ACpass>" +
                $"<xmlInvData><![CDATA[{xml}]]></xmlInvData>" +
                $"<username>{wsAccount}</username>" +
                $"<pass>{wsPass}</pass>" +
                $"<fkey>{id}</fkey>" +
                $"<AttachFile>0</AttachFile>" +
                $"<pattern>{pattern}</pattern>" +
                $"<serial>{serial}</serial>" +
                $"<convert>0</convert>" +
                $"</ReplaceInvoiceAction>" +
                $"</soap:Body>" +
                $"</soap:Envelope>";

            document.LoadXml(xmlString);
            return document;
        }

        public static XmlDocument AdjustInvoiceAction(string id, string adminAccount,
            string adminPass,
            string xml,
            string wsAccount,
            string wsPass,
            string pattern,
            string serial)
        {
            XmlDocument document = new XmlDocument();
            string xmlString = $"<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                $"<soap:Body>" +
                $"<AdjustInvoiceAction xmlns=\"http://tempuri.org/\">" +
                $"<Account>{adminAccount}</Account>" +
                $"<ACpass>{adminPass}</ACpass>" +
                $"<xmlInvData><![CDATA[{xml}]]></xmlInvData>" +
                $"<username>{wsAccount}</username>" +
                $"<pass>{wsPass}</pass>" +
                $"<fkey>{id}</fkey>" +
                $"<AttachFile>0</AttachFile>" +
                $"<pattern>{pattern}</pattern>" +
                $"<serial>{serial}</serial>" +
                $"<convert>0</convert>" +
                $"</AdjustInvoiceAction>" +
                $"</soap:Body>" +
                $"</soap:Envelope>";

            document.LoadXml(xmlString);
            return document;
        }

        public static XmlDocument ImportAndPublishInv(string adminAccount,
            string adminPass,
            string xml,
            string wsAccount,
            string wsPass,
            string serialCert,
            string pattern,
            string serial)
        {
            XmlDocument document = new XmlDocument();
            string xmlString = $"<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                $"<soap:Body>" +
                $"<ImportAndPublishInv xmlns=\"http://tempuri.org/\">" +
                $"<Account>{adminAccount}</Account>" +
                $"<ACpass>{adminPass}</ACpass>" +
                $"<xmlInvData><![CDATA[{xml}]]></xmlInvData>" +
                $"<username>{wsAccount}</username>" +
                $"<password>{wsPass}</password>" +
                $"<pattern>{pattern}</pattern>" +
                $"<serial>{serial}</serial>" +
                $"<convert>0</convert>" +
                $"</ImportAndPublishInv>" +
                $"</soap:Body>" +
                $"</soap:Envelope>";

            document.LoadXml(xmlString);
            return document;
        }

        public static XmlDocument downloadInvPDFFkey(string fkey,string wsAccount,string wsPass)
        {
            XmlDocument document = new XmlDocument();
            string xmlString = $"<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                $"<soap:Body>" +
                $"<downloadInvPDFFkey  xmlns=\"http://tempuri.org/\">" +
                $"<fkey>{fkey}</fkey>" +
                $"<userName>{wsAccount}</userName>" +
                $"<userPass>{wsPass}</userPass>" +  
                $"</downloadInvPDFFkey >" +
                $"</soap:Body>" +
                $"</soap:Envelope>";

            document.LoadXml(xmlString);
            return document;
        }
        
              public static XmlDocument cancelInv(string adminAccount,string adminPass, string fkey, string wsAccount, string wsPass)
        {
            XmlDocument document = new XmlDocument();
            string xmlString = $"<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                $"<soap:Body>" +
                $"<cancelInv  xmlns=\"http://tempuri.org/\">" +
                $"<Account>{adminAccount}</Account>" +
                $"<ACpass>{adminPass}</ACpass>" +
                $"<fkey>{fkey}</fkey>" +
                $"<userName>{wsAccount}</userName>" +
                $"<userPass>{wsPass}</userPass>" +
                $"</cancelInv >" +
                $"</soap:Body>" +
                $"</soap:Envelope>";

            document.LoadXml(xmlString);
            return document;
        }
        public static XmlDocument confirmPaymentFkey(string fkey, string wsAccount, string wsPass)
        {
            XmlDocument document = new XmlDocument();
            string xmlString = $"<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                $"<soap:Body>" +
                $"<confirmPaymentFkey  xmlns=\"http://tempuri.org/\">" +
                $"<lstFkey>{fkey}</lstFkey>" +
                $"<userName>{wsAccount}</userName>" +
                $"<userPass>{wsPass}</userPass>" +
                $"</confirmPaymentFkey >" +
                $"</soap:Body>" +
                $"</soap:Envelope>";

            document.LoadXml(xmlString);
            return document;
        }
    }
}
