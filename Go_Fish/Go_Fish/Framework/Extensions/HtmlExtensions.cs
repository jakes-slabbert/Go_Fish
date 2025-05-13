using HtmlTags;
using Fortis.ProFramework.Enumerations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fortis.ProFramework.Extensions
{
    public static class HtmlExtensions
    {
        public static HtmlExtensions<TModel> HtmlExt<TModel>(this IHtmlHelper<TModel> helper)
        {
            return new HtmlExtensions<TModel>(helper);
        }
    }

    public class HtmlExtensions<TModel>
    {
        protected readonly IHtmlHelper Helper;

        public HtmlExtensions(IHtmlHelper helper)
        {
            Helper = helper;
        }

        /// <summary>
        /// Begins the box.
        /// </summary>
        /// <param name="colourClasses">The colour classes.</param>
        /// <returns></returns>
        public IDisposable BeginBox(string cssClass = "")
        {
            Helper.ViewContext.Writer.Write(new HtmlTag("div").AddClasses("box", cssClass).NoClosingTag().ToString());
            return new HtmlCloseTag(Helper, "</div>");
        }
        public IDisposable BeginBox(BoxStyles cssClass = BoxStyles.Default)
        {
            Helper.ViewContext.Writer.Write(new HtmlTag("div").AddClasses("box", cssClass.ToString()).NoClosingTag().ToString());
            return new HtmlCloseTag(Helper, "</div>");
        }


        /// <summary>
        /// Boxes the header.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="withBorder">if set to <c>true</c> [with border].</param>
        /// <returns></returns>
        public HtmlTag BoxHeader(string title, bool withBorder = false)
        {
            //<div class="box-header with-border">
            //  <h3 class="box-title">Default Box Example</h3>
            //  <div class="box-tools pull-right">
            //      <!-- Buttons, labels, and many other things can be placed here! -->
            //      <!-- Here is a label for example -->
            //      <span class="label label-primary">Label</span>
            //  </div>
            //  <!-- /.box-tools -->
            //</div>

            var headerBox = new HtmlTag("div");
            headerBox.AddClass("card-header");

            if (withBorder)
                headerBox.AddClass("with-border");

            var titleHeading = new HtmlTag("h3");
            titleHeading.AddClass("card-title");
            titleHeading.Text(title);


            return headerBox.Append(titleHeading);
        }

        /// <summary>
        /// Boxes the body.
        /// </summary>
        /// <returns></returns>
        public IDisposable BoxBody()
        {
            var div = new HtmlTag("div");
            div.AddClass("card-body");
            div.NoClosingTag();
            Helper.ViewContext.Writer.Write(div.ToString());
            return new HtmlCloseTag(Helper, "</div>");
        }

        //<div class="box box-primary">
        //    <div class="box-header with-border">
        //      <h3 class="box-title">Quick Example</h3>
        //    </div>
        //    <!-- /.box-header -->
        //    <!-- form start -->
        //    <form role = "form" lpformnum= "1" >
        //      < div class="box-body">
        //        <div class="form-group">
        //          <label for="exampleInputEmail1">Email address</label>
        //          <input type = "email" class="form-control" id="exampleInputEmail1" placeholder="Enter email" style="background-image: url(&quot;data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAASCAYAAABSO15qAAAAAXNSR0IArs4c6QAAAPhJREFUOBHlU70KgzAQPlMhEvoQTg6OPoOjT+JWOnRqkUKHgqWP4OQbOPokTk6OTkVULNSLVc62oJmbIdzd95NcuGjX2/3YVI/Ts+t0WLE2ut5xsQ0O+90F6UxFjAI8qNcEGONia08e6MNONYwCS7EQAizLmtGUDEzTBNd1fxsYhjEBnHPQNG3KKTYV34F8ec/zwHEciOMYyrIE3/ehKAqIoggo9inGXKmFXwbyBkmSQJqmUNe15IRhCG3byphitm1/eUzDM4qR0TTNjEixGdAnSi3keS5vSk2UDKqqgizLqB4YzvassiKhGtZ/jDMtLOnHz7TE+yf8BaDZXA509yeBAAAAAElFTkSuQmCC&quot;); background-repeat: no-repeat; background-attachment: scroll; background-size: 16px 18px; background-position: 98% 50%;" autocomplete="off">
        //        </div>
        //        <div class="form-group">
        //          <label for="exampleInputPassword1">Password</label>
        //          <input type = "password" class="form-control" id="exampleInputPassword1" placeholder="Password" style="background-image: url(&quot;data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAASCAYAAABSO15qAAAAAXNSR0IArs4c6QAAAPhJREFUOBHlU70KgzAQPlMhEvoQTg6OPoOjT+JWOnRqkUKHgqWP4OQbOPokTk6OTkVULNSLVc62oJmbIdzd95NcuGjX2/3YVI/Ts+t0WLE2ut5xsQ0O+90F6UxFjAI8qNcEGONia08e6MNONYwCS7EQAizLmtGUDEzTBNd1fxsYhjEBnHPQNG3KKTYV34F8ec/zwHEciOMYyrIE3/ehKAqIoggo9inGXKmFXwbyBkmSQJqmUNe15IRhCG3byphitm1/eUzDM4qR0TTNjEixGdAnSi3keS5vSk2UDKqqgizLqB4YzvassiKhGtZ/jDMtLOnHz7TE+yf8BaDZXA509yeBAAAAAElFTkSuQmCC&quot;); background-repeat: no-repeat; background-attachment: scroll; background-size: 16px 18px; background-position: 98% 50%;" autocomplete="off">
        //        </div>
        //        <div class="form-group">
        //          <label for="exampleInputFile">File input</label>
        //          <input type = "file" id= "exampleInputFile" >

        //          < p class="help-block">Example block-level help text here.</p>
        //        </div>
        //        <div class="checkbox">
        //          <label>
        //            <input type = "checkbox" > Check me out
        //          </label>
        //        </div>
        //      </div>
        //      <!-- /.box-body -->

        //      <div class="box-footer">
        //        <button type = "submit" class="btn btn-primary">Submit</button>
        //      </div>
        //    </form>
        //  </div>
    }

    /// <summary>
    /// Used to close a HTML Tag when the @using statement was used
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class HtmlCloseTag : IDisposable
    {
        IHtmlHelper _helper;
        private string _closingTag;

        /// <summary>
        /// Admins the lte box.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="closingTag">The closing tag.</param>
        public HtmlCloseTag(IHtmlHelper helper, string closingTag)
        {
            _helper = helper;
            _closingTag = closingTag;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _helper.ViewContext.Writer.Write(_closingTag);
        }
    }
}
