using System ;
using System.Text ;
using System.Net.Sockets ;
using System.IO ;

using System.Xml;
using System.Xml.Serialization;

namespace LegitimeAPP.Backdoor {
    public class Data<DataType> {
        public static DataType XmlToData(string data) {
            var serializer = new XmlSerializer(typeof(DataType));
            DataType result;

            using (TextReader reader = new StringReader(data))
            {
                result = (DataType)serializer.Deserialize(reader);
            }

            return result;
        }

        public static string DataToXml(DataType data) {

            string resultat = "";

            var serializer = new XmlSerializer(typeof(DataType));

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, data);
                    resultat = sww.ToString();
                }
            }
            return resultat;
        }
    }
}