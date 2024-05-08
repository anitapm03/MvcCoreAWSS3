using Amazon.S3;
using Amazon.S3.Model;

namespace MvcCoreAWSS3.Services
{
    public class ServiceStorageS3
    {

        //vamos a recibir el nombre del bucket 
        //desde app settings
        private string BucketName;

        //la clase/interfaz para los buckets se
        //llama IAmazonS3 y la recibimos por inyeccion
        private IAmazonS3 ClientS3;

        public ServiceStorageS3(IConfiguration configuration,
            IAmazonS3 clientS3)
        {
            this.ClientS3 = clientS3;
            this.BucketName = configuration.GetValue<string>
                ("AWS:BucketName");
        }

        //comenzamos subiendo un ficheto al bucket
        //necesitamos nombre y stream 
        public async Task<bool> UploadFileAsync
            (string fileName, Stream stream)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = this.BucketName,
                Key = fileName,
                InputStream = stream
            };

            //para ejecutarlo debemos hacer una peticion al 
            //client s3 y nos devolvera un response del mismo
            //tipo que el request
            PutObjectResponse response = await
                this.ClientS3.PutObjectAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //metodo para eliminar fichero del bucket
        public async Task<bool> DeleteFileAsync
            (string fileName)
        {
            //podemos hacer peticiones sin request
            DeleteObjectResponse response = await
                this.ClientS3.DeleteObjectAsync
                (this.BucketName, fileName);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //para recuperar todos los ficheros (URL) se realiza
        //mediante versiones. aunque no tengamos habilitado el 
        //control de versiones, las keys siempre van por version
        public async Task<List<string>> GetVersionesFileAsync()
        {
            //primero requperamos la respuesta con 
            //todas las versiones a partir de un bucket
            ListVersionsResponse response = await
                this.ClientS3.ListVersionsAsync(this.BucketName);
            //extraemos las keys de nuestros ficheros
            List<string> keyFiles = response.Versions
                .Select(x => x.Key).ToList();

            return keyFiles;
        }


    }
}
