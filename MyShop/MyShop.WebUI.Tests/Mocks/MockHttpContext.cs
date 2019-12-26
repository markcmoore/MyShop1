using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static MyShop.WebUI.Tests.Mocks.MockResponse;

namespace MyShop.WebUI.Tests.Mocks
{
    //this is the first class that the next 2 are used by....
    public class MockHttpContext : HttpContextBase
    {
        private MockRequest  request;
        private MockResponse response;
        private HttpCookieCollection cookies;

        public MockHttpContext()
        {
            cookies = new HttpCookieCollection();
            this.response = new MockResponse(cookies);
            this.request = new MockRequest(cookies);
        }

        public override HttpResponseBase Response
        {
            get { return response; }
        }

        public override HttpRequestBase Request
        {
            get { return request; }
        }

    }

    //2nd class...
    public class MockResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection cookies;

        //constructor
        public MockResponse(HttpCookieCollection cookies)
        {
            this.cookies = cookies;
        }

        //variable
        public override HttpCookieCollection Cookies
        {
            get { return cookies; }
        }

        //3rd class
        public class MockRequest : HttpRequestBase
        {
            private readonly HttpCookieCollection cookies;

            //constructor
            public MockRequest(HttpCookieCollection cookies)
            {
                this.cookies = cookies;
            }

            //variable
            public override HttpCookieCollection Cookies
            {
                get { return cookies; }
            }


        }
    }
}
