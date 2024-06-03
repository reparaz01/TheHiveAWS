using Amazon.S3.Model;
using Amazon.S3;
using TheHiveAWS.Models;

namespace TheHiveAWS.Services
{
    public class ServiceStorageAWS
    {
        private IAmazonS3 client;
        private string BucketName;

        public ServiceStorageAWS(KeysModel keys, IAmazonS3 client)
        {
            this.BucketName = keys.BucketName;
            this.client = client;
        }

        //METODO PARA SUBIR LAS IMAGENES DONDE NECESITAMOS
        //EL NOMBRE DE LA IMAGEN, EL STREAM, EL BUCKET NAME
        public async Task<bool> UploadFileAsync(string fileName, Stream stream)
        {
            string key = "Publicaciones/" + fileName;

            PutObjectRequest request = new PutObjectRequest
            {
                InputStream = stream,
                Key = key,
                BucketName = this.BucketName
            };
            PutObjectResponse response = await this.client.PutObjectAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }


        public async Task<bool> UploadUserProfilePictureAsync(string fileName, Stream stream)
        {
            string key = "Usuarios/" + fileName;

            PutObjectRequest request = new PutObjectRequest
            {
                InputStream = stream,
                Key = key,
                BucketName = this.BucketName
            };
            PutObjectResponse response = await this.client.PutObjectAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

    }
}
