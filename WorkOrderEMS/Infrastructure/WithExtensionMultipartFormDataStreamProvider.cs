using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using WorkOrderEMS.Infrastructure.Util;

namespace WorkOrderEMS.Infrastructure
{
	public class WithExtensionMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
	{
		public WithExtensionMultipartFormDataStreamProvider(string rootPath)
			: base(rootPath)
		{
		}

		public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
		{
			string extension = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? Path.GetExtension(OSUtil.GetValidFileName(headers.ContentDisposition.FileName)) : "";
			return Guid.NewGuid().ToString() + extension;
		}
	}
}