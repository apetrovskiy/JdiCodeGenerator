namespace JdiCodeGenerator.Tests.Recognition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Helpers;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using HtmlAgilityPack;
    using Internals;
    using Xunit;

    public class Bootstrap3
    {
        CodeEntry _entry;
        HtmlDocument _doc;
        readonly List<ICodeEntry> _entries;

        public Bootstrap3()
        {
            _entry = null;
            _doc = null;
            _entries = new List<ICodeEntry>();
        }

        [Theory]
        [InlineData(@"
<button type=""button"" class=""btn btn-default"" aria-label=""Left Align"">
  <span class=""glyphicon glyphicon-align-left"" aria-hidden=""true""></span>
</button>
", "IButton", 0)]

        [InlineData(@"
<button type=""button"" class=""btn btn-default btn-lg"">
  <span class=""glyphicon glyphicon-star"" aria-hidden=""true""></span> Star
</button>
", "IButton", 0)]
        [InlineData(@"
<div class=""alert alert-danger"" role=""alert"">
  <span class=""glyphicon glyphicon-exclamation-sign"" aria-hidden=""true""></span>
  <span class=""sr-only"">Error:</span>
  Enter a valid email address
</div>
", "IText", 0)] // alert
        [InlineData(@"
<button type=""button"" class=""btn btn-default navbar-btn"">Sign in</button>
", "IButton", 0)]
        [InlineData(@"
<p class=""navbar-text"">Signed in as Mark Otto</p>
", "IText", 0)]
        [InlineData(@"
<p class=""navbar-text navbar-right"">Signed in as <a href=""#"" class=""navbar-link"">Mark Otto</a></p>
", "IText", 0)]
        [InlineData(@"
<h3>Example heading <span class=""label label-default"">New</span></h3>
", "ILabel", 0)]
        [InlineData(@"
<span class=""label label-default"">Default</span>
", "ILabel", 0)]
        [InlineData(@"
<span class=""label label-primary"">Primary</span>
", "ILabel", 0)]
        [InlineData(@"
<span class=""label label-success"">Success</span>
", "ILabel", 0)]
        [InlineData(@"
<span class=""label label-info"">Info</span>
", "ILabel", 0)]
        [InlineData(@"
<span class=""label label-warning"">Warning</span>
", "ILabel", 0)]
        [InlineData(@"
<span class=""label label-danger"">Danger</span>
", "ILabel", 0)]
        [InlineData(@"
<a href=""#"">Inbox <span class=""badge"">42</span></a>

<button class=""btn btn-primary"" type=""button"">
  Messages <span class=""badge"">4</span>
</button>
", "ILink", 0)] // Badges
        [InlineData(@"
<ul class=""nav nav-pills"" role=""tablist"">
  <li role=""presentation"" class=""active""><a href=""#"">Home <span class=""badge"">42</span></a></li>
  <li role=""presentation""><a href=""#"">Profile</a></li>
  <li role=""presentation""><a href=""#"">Messages <span class=""badge"">3</span></a></li>
</ul>
", "ILink", 2)]
        [InlineData(@"
<div class=""jumbotron"">
  <h1>Hello, world!</h1>
  <p>...</p>
  <p><a class=""btn btn-primary btn-lg"" href=""#"" role=""button"">Learn more</a></p>
</div>
", "IText", 0)] // Jumbotron

           [InlineData(@"
<div class=""jumbotron"">
  <div class=""container"">
    ...
  </div>
</div>
", "IText", 0)]

        [InlineData(@"
<div class=""page-header"">
  <h1>Example page header <small>Subtext for header</small></h1>
</div>
", "IText", 0)] // Page header
        [InlineData(@"
<div class=""alert alert-success"" role=""alert"">...</div>
", "IText", 0)] // Alerts
        [InlineData(@"
<div class=""alert alert-info"" role=""alert"">...</div>
", "IText", 0)]
        [InlineData(@"
<div class=""alert alert-warning"" role=""alert"">...</div>
", "IText", 0)]
        [InlineData(@"
<div class=""alert alert-danger"" role=""alert"">...</div>
", "IText", 0)]
        
[InlineData(@"
<div class=""alert alert-warning alert-dismissible"" role=""alert"">
  <button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Close""><span aria-hidden=""true"">&times;</span></button>
  <strong>Warning!</strong> Better check yourself, you're not looking too good.
</div>
", "IText", 0)] // Dismissible alerts
[InlineData(@"
<div class=""alert alert-success"" role=""alert"">
  <a href=""#"" class=""alert-link"">...</a>
</div>
", "ILink", 1)] // Links in alerts
        [InlineData(@"
<div class=""alert alert-info"" role=""alert"">
  <a href=""#"" class=""alert-link"">...</a>
</div>
", "ILink", 1)]
[InlineData(@"
<div class=""alert alert-warning"" role=""alert"">
  <a href=""#"" class=""alert-link"">...</a>
</div>
", "ILink", 1)]
[InlineData(@"
<div class=""alert alert-danger"" role=""alert"">
  <a href=""#"" class=""alert-link"">...</a>
</div>
", "ILink", 1)]
        [InlineData(@"
<div class=""well"">...</div>
", "IText", 0)] // Wells // Default well
        [InlineData(@"
<div class=""well well - lg"">...</div>
", "IText", 0)] // Optional classes
        [InlineData(@"
<div class=""well well - sm"">...</div>
", "IText", 0)]

        [InlineData(@"
<div class=""btn-group"" role=""group"" aria-label=""..."">
<button type=""button"" class=""btn btn-default"">Left</button>
<button type=""button"" class=""btn btn-default"">Middle</button>
<button type=""button"" class=""btn btn-default"">Right</button>
</div>
", "IButton", 1)] // button groups
        [InlineData(@"
<div class=""btn-group"" role=""group"" aria-label=""..."">
<button type=""button"" class=""btn btn-default"">Left</button>
<button type=""button"" class=""btn btn-default"">Middle</button>
<button type=""button"" class=""btn btn-default"">Right</button>
</div>
", "IButton", 2)] // button groups
        [InlineData(@"
<div class=""btn-group"" role=""group"" aria-label=""..."">
<button type=""button"" class=""btn btn-default"">Left</button>
<button type=""button"" class=""btn btn-default"">Middle</button>
<button type=""button"" class=""btn btn-default"">Right</button>
</div>
", "IButton", 3)] // button groups
        [InlineData(@"
<div class=""btn-toolbar"" role=""toolbar"" aria-label=""..."">
<div class=""btn-group"" role=""group"" aria-label=""...""><button type=""button"" class=""btn btn-default"">Button1</button></div>
<div class=""btn-group"" role=""group"" aria-label=""..."">...</div>
<div class=""btn-group"" role=""group"" aria-label=""..."">...</div>
</div>
", "IButton", 2)] // button toolbar
        [InlineData(@"
<div class=""btn-toolbar"" role=""toolbar"" aria-label=""..."">
<div class=""btn-group"" role=""group"" aria-label=""..."">...</div>
<div class=""btn-group"" role=""group"" aria-label=""...""><button type=""button"" class=""btn btn-default"">Button1</button></div>
<div class=""btn-group"" role=""group"" aria-label=""..."">...</div>
</div>
", "IButton", 3)] // button toolbar
        [InlineData(@"
<div class=""btn-toolbar"" role=""toolbar"" aria-label=""..."">
<div class=""btn-group"" role=""group"" aria-label=""..."">...</div>
<div class=""btn-group"" role=""group"" aria-label=""..."">...</div>
<div class=""btn-group"" role=""group"" aria-label=""...""><button type=""button"" class=""btn btn-default"">Button1</button></div>
</div>
", "IButton", 4)] // button toolbar
        [InlineData(@"
<div class=""btn-group btn-group-lg"" role=""group"" aria-label=""...""><button type=""button"" class=""btn btn-default"">Button1</button></div>
", "IButton", 1)] // sizing
        [InlineData(@"
<div class=""btn-group"" role=""group"" aria-label=""...""><button type=""button"" class=""btn btn-default"">Button1</button></div>
", "IButton", 1)] // sizing
        [InlineData(@"
<div class=""btn-group btn-group-sm"" role=""group"" aria-label=""...""><button type=""button"" class=""btn btn-default"">Button1</button></div>
", "IButton", 1)] // sizing
        [InlineData(@"
<div class=""btn-group btn-group-xs"" role=""group"" aria-label=""...""><button type=""button"" class=""btn btn-default"">Button1</button></div>
", "IButton", 1)] // sizing
        #region commented
        /*
        [InlineData(@"

        ", "")]

        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]

        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]

        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]

        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]

        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]

        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]

        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]

        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        [InlineData(@"

        ", "")]
        */
        #endregion
        [InlineData(@"
<div class=""input-group"">
<span class=""input-group-addon"" id=""basic-addon1"">@</span>
<input type = ""text"" class=""form-control"" placeholder=""Username"" aria-describedby=""basic-addon1"">
</div>
", "ITextField", 2)]
        [InlineData(@"
<div class=""input-group"">
<input type = ""text"" class=""form-control"" placeholder=""Recipient's username"" aria-describedby=""basic-addon2"">
<span class=""input-group-addon"" id=""basic-addon2"">@example.com</span>
</div>
", "ITextField", 1)]
        [InlineData(@"
< div class=""input-group"">
<span class=""input-group-addon"">$</span>
<input type = ""text"" class=""form-control"" aria-label=""Amount(to the nearest dollar)"">
<span class=""input-group-addon"">.00</span>
</div>
", "ITextField", 2)]
        [InlineData(@"
<label for=""basic-url"">Your vanity URL</label>
<div class=""input-group"">
<span class=""input-group-addon"" id=""basic-addon3"">https://example.com/users/</span>
<input type = ""text"" class=""form-control"" id=""basic-url"" aria-describedby=""basic-addon3"">
</div>
", "ITextField", 3)]

        [InlineData(@"
<form class=""navbar-form navbar-left"" role=""search"">
    <div class=""row"">
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <span class=""input-group-addon"">
        <input type=""checkbox"" aria-label=""..."">
      </span>
      <input type=""text"" class=""form-control"" aria-label=""..."">
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <span class=""input-group-addon"">
        <input type=""radio"" aria-label=""..."">
      </span>
      <input type=""text"" class=""form-control"" aria-label=""..."">
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    </div><!-- /.row -->
</form>
", "ICheckBox", 5)] // Checkboxes and radio addons
//        [InlineData(@"
//<form class=""navbar-form navbar-left"" role=""search"">
//    <div class=""row"">
//    <div class=""col-lg-6"">
//    <div class=""input-group"">
//      <span class=""input-group-addon"">
//        <input type=""checkbox"" aria-label=""..."">
//      </span>
//      <input type=""text"" class=""form-control"" aria-label=""..."">
//    </div><!-- /input-group -->
//    </div><!-- /.col-lg-6 -->
//    <div class=""col-lg-6"">
//    <div class=""input-group"">
//      <span class=""input-group-addon"">
//        <input type=""radio"" aria-label=""..."">
//      </span>
//      <input type=""text"" class=""form-control"" aria-label=""..."">
//    </div><!-- /input-group -->
//    </div><!-- /.col-lg-6 -->
//    </div><!-- /.row -->
//</form>
//", "IRadioButtons", 10)] // Checkboxes and radio addons
        [InlineData(@"
<form class=""navbar-form navbar-left"" role=""search"">
    <div class=""row"">
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <span class=""input-group-btn"">
        <button class=""btn btn-default"" type=""button"">Go!</button>
      </span>
      <input type=""text"" class=""form-control"" placeholder=""Search for..."">
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <input type=""text"" class=""form-control"" placeholder=""Search for..."">
      <span class=""input-group-btn"">
        <button class=""btn btn-default"" type=""button"">Go!</button>
      </span>
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    </div><!-- /.row -->
</form>
", "IButton", 11)] // Button addons
        [InlineData(@"
<form class=""navbar-form navbar-left"" role=""search"">
    <div class=""row"">
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <div class=""input-group-btn"">
        <button type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">Action <span class=""caret""></span></button>
        <ul class=""dropdown-menu"">
          <li><a href=""#"">Action</a></li>
          <li><a href=""#"">Another action</a></li>
          <li><a href=""#"">Something else here</a></li>
          <li role=""separator"" class=""divider""></li>
          <li><a href=""#"">Separated link</a></li>
        </ul>
      </div><!-- /btn-group -->
      <input type=""text"" class=""form-control"" aria-label=""..."">
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <input type=""text"" class=""form-control"" aria-label=""..."">
      <div class=""input-group-btn"">
        <button type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">Action <span class=""caret""></span></button>
        <ul class=""dropdown-menu dropdown-menu-right"">
          <li><a href=""#"">Action</a></li>
          <li><a href=""#"">Another action</a></li>
          <li><a href=""#"">Something else here</a></li>
          <li role=""separator"" class=""divider""></li>
          <li><a href=""#"">Separated link</a></li>
        </ul>
      </div><!-- /btn-group -->
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    </div><!-- /.row -->
</form>
", "IButton", 5)] // Buttons with dropdowns
        [Trait("Category", "Bootstrap 3, single element")]
        public void ParseBootstrap3ForSingleElement(string input, string expected, int elementPosition)
        {
            GivenHtml(input);
            WhenParsing(elementPosition);
            ThenThereIsElementOfType(expected);
        }
        
        [Theory]
        [InlineData(@"
<div class=""dropdown"">
  <button class=""btn btn-default dropdown-toggle"" type=""button"" id=""dropdownMenu1"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""true"">
    Dropdown
    <span class=""caret""></span>
  </button>
  <ul class=""dropdown-menu"" aria-labelledby=""dropdownMenu1"">
    <li><a href=""#"">Action</a></li>
    <li><a href=""#"">Another action</a></li>
    <li><a href=""#"">Something else here</a></li>
    <li role=""separator"" class=""divider""></li>
    <li><a href=""#"">Separated link</a></li>
  </ul>
</div>
", "IDropDown<SomeEnum>", 0)]
    [InlineData(@"
<div class=""dropup"">
<button class=""btn btn-default dropdown-toggle"" type=""button"" id=""dropdownMenu2"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
Dropup
<span class=""caret""></span>
</button>
<ul class=""dropdown-menu"" aria-labelledby=""dropdownMenu2"">
<li><a href=""#"">Action</a></li>
<li><a href=""#"">Another action</a></li>
<li><a href=""#"">Something else here</a></li>
<li role=""separator"" class=""divider""></li>
<li><a href=""#"">Separated link</a></li>
</ul>
</div>
", "IDropDown<SomeEnum>", 0)]

    [InlineData(@"
<div class=""dropdown"">
    <button class=""btn btn-default dropdown-toggle"" />
    <ul class=""dropdown-menu dropdown-menu-right"" aria-labelledby=""dLabel"">
    <li><a href=""#"">Action</a></li>
    <li><a href=""#"">Another action</a></li>
    </ul>
</div>
", "IDropDown<SomeEnum>", 0)] // alignment
    [InlineData(@"
<div class=""dropdown"">
    <button class=""btn btn-default dropdown-toggle"" />
    <ul class=""dropdown-menu"" aria-labelledby=""dropdownMenu3"">
    ...
    <li class=""dropdown-header"">Dropdown header</li>
    ...
    </ul>
</div>
", "IDropDown<SomeEnum>", 0)] // headers
    [InlineData(@"
<div class=""dropdown"">
    <ul class=""dropdown-menu"" aria-labelledby=""dropdownMenuDivider"">
    ...
    <li role=""separator"" class=""divider""></li>
    ...
    </ul>
</div>
", "IElement", 2)] // divider
    [InlineData(@"
<div class=""dropdown"">
    <ul class=""dropdown-menu"" aria-labelledby=""dropdownMenu4"">
    <li><a href=""#"">Regular link</a></li>
    <li class=""disabled""><a href=""#"">Disabled link</a></li>
    <li><a href=""#"">Another link</a></li>
    </ul>
</div>
", "ILink", 3)] // disabled menu items

        [InlineData(@"
<div class=""btn-group"" role=""group"" aria-label=""..."">
<button type=""button"" class=""btn btn-default"">1</button>
<button type=""button"" class=""btn btn-default"">2</button>

<div class=""btn-group"" role=""group"">
<button type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
  Dropdown
  <span class=""caret""></span>
</button>
<ul class=""dropdown-menu"">
  <li><a href=""#"">Dropdown link</a></li>
  <li><a href=""#"">Dropdown link</a></li>
</ul>
</div>
</div>
", "IDropDown<SomeEnum>", 3)] // nesting
    [InlineData(@"
<!--
<div class=""btn-group-vertical"" role=""group"" aria-label=""..."">
...
</div>
-->
<div class=""bs-example"" data-example-id=""vertical-button-group"">
    <div class=""btn-group-vertical"" role=""group"" aria-label=""Vertical button group"">
        <button type=""button"" class=""btn btn-default"">Button</button>
        <button type=""button"" class=""btn btn-default"">Button</button>
        <div class=""btn-group"" role=""group"">
            <button id=""btnGroupVerticalDrop1"" type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false""> Dropdown <span class=""caret""></span>
            </button>
            <ul class=""dropdown-menu"" aria-labelledby=""btnGroupVerticalDrop1"">
                <li><a href=""#"">Dropdown link</a></li>
                <li><a href=""#"">Dropdown link</a></li>
            </ul>
        </div>
        <button type=""button"" class=""btn btn-default"">Button</button>
        <button type=""button"" class=""btn btn-default"">Button</button>
        <div class=""btn-group"" role=""group"">
            <button id=""btnGroupVerticalDrop2"" type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false""> Dropdown <span class=""caret""></span> </button>
            <ul class=""dropdown-menu"" aria-labelledby=""btnGroupVerticalDrop2"">
                <li><a href=""#"">Dropdown link</a></li>
                <li><a href=""#"">Dropdown link</a></li>
            </ul>
        </div>
        <div class=""btn-group"" role=""group"">
            <button id=""btnGroupVerticalDrop3"" type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false""> Dropdown <span class=""caret""></span> </button>
            <ul class=""dropdown-menu"" aria-labelledby=""btnGroupVerticalDrop3"">
                <li><a href=""#"">Dropdown link</a></li> <li><a href=""#"">Dropdown link</a></li>
            </ul>
        </div>
        <div class=""btn-group"" role=""group"">
            <button id=""btnGroupVerticalDrop4"" type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false""> Dropdown <span class=""caret""></span> </button>
            <ul class=""dropdown-menu"" aria-labelledby=""btnGroupVerticalDrop4"">
                <li><a href=""#"">Dropdown link</a></li>
                <li><a href=""#"">Dropdown link</a></li>
            </ul>
        </div>
    </div>
</div>
", "IDropDown<SomeEnum>", 4)] // vertical variation

    [InlineData(@"
<!--<div class=""btn-group btn-group-justified"" role=""group"" aria-label=""..."">
...
</div>-->
<div class=""bs-example"" data-example-id=""simple-justified-button-group""> <div class=""btn-group btn-group-justified"" role=""group"" aria-label=""Justified button group""> <a href=""#"" class=""btn btn-default"" role=""button"">Left</a> <a href=""#"" class=""btn btn-default"" role=""button"">Middle</a> <a href=""#"" class=""btn btn-default"" role=""button"">Right</a> </div> <br> <div class=""btn-group btn-group-justified"" role=""group"" aria-label=""Justified button group with nested dropdown""> <a href=""#"" class=""btn btn-default"" role=""button"">Left</a> <a href=""#"" class=""btn btn-default"" role=""button"">Middle</a> <div class=""btn-group"" role=""group""> <a href=""#"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" role=""button"" aria-haspopup=""true"" aria-expanded=""false""> Dropdown <span class=""caret""></span> </a> <ul class=""dropdown-menu""> <li><a href=""#"">Action</a></li> <li><a href=""#"">Another action</a></li> <li><a href=""#"">Something else here</a></li> <li role=""separator"" class=""divider""></li> <li><a href=""#"">Separated link</a></li> </ul> </div> </div> </div>
", "IDropDown<SomeEnum>", 8)] // justified button groups

//    [InlineData(@"
//<div class="btn-group btn-group-justified" role="group" aria-label="...">
//<div class="btn-group" role="group">
//<button type="button" class="btn btn-default">Left</button>
//</div>
//<div class="btn-group" role="group">
//<button type="button" class="btn btn-default">Middle</button>
//</div>
//<div class="btn-group" role="group">
//<button type="button" class="btn btn-default">Right</button>
//</div>
//</div>
//", "")]
    [InlineData(@"
<!-- Single button -->
<div class=""btn-group"">
<button type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
Action <span class=""caret""></span>
</button>
<ul class=""dropdown-menu"">
<li><a href=""#"">Action</a></li>
<li><a href=""#"">Another action</a></li>
<li><a href=""#"">Something else here</a></li>
<li role=""separator"" class=""divider""></li>
<li><a href=""#"">Separated link</a></li>
</ul>
</div>
", "IDropDown<SomeEnum>", 0)] // button dropdowns

    [InlineData(@"
<!-- Split button -->
<div class=""btn-group"">
<button type=""button"" class=""btn btn-danger"">Action</button>
<button type=""button"" class=""btn btn-danger dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
<span class=""caret""></span>
<span class=""sr-only"">Toggle Dropdown</span>
</button>
<ul class=""dropdown-menu"">
<li><a href=""#"">Action</a></li>
<li><a href=""#"">Another action</a></li>
<li><a href=""#"">Something else here</a></li>
<li role=""separator"" class=""divider""></li>
<li><a href=""#"">Separated link</a></li>
</ul>
</div>
", "IDropDown<SomeEnum>", 0)] // split button dropdowns

    [InlineData(@"
<!-- Large button group -->
<div class=""btn-group"">
<button class=""btn btn-default btn-lg dropdown-toggle"" type=""button"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
Large button <span class=""caret""></span>
</button>
<ul class=""dropdown-menu"">
...
</ul>
</div>
", "IDropDown<SomeEnum>", 0)]
        [InlineData(@"
<!-- Small button group -->
<div class=""btn-group"">
<button class=""btn btn-default btn-sm dropdown-toggle"" type=""button"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
Small button <span class=""caret""></span>
</button>
<ul class=""dropdown-menu"">
...
</ul>
</div>
", "IDropDown<SomeEnum>", 0)]
        [InlineData(@"
<!-- Extra small button group -->
<div class=""btn-group"">
<button class=""btn btn-default btn-xs dropdown-toggle"" type=""button"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
Extra small button <span class=""caret""></span>
</button>
<ul class=""dropdown-menu"">
...
</ul>
</div>
", "IDropDown<SomeEnum>", 0)] // sizing x3

    [InlineData(@"
<div class=""btn-group dropup"">
<button type=""button"" class=""btn btn-default"">Dropup</button>
<button type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
<span class=""caret""></span>
<span class=""sr-only"">Toggle Dropdown</span>
</button>
<ul class=""dropdown-menu"">
<!-- Dropdown menu links -->
</ul>
</div>
", "IDropDown<SomeEnum>", 0)] // dropup variation

    [InlineData(@"
<form class=""navbar-form navbar-left"" role=""search"">
    <div class=""input-group"">
    <span class=""input-group-addon"" id=""basic-addon1"">@</span>
    <input type=""text"" class=""form-control"" placeholder=""Username"" aria-describedby=""basic-addon1"">
    </div>

    <div class=""input-group"">
    <input type=""text"" class=""form-control"" placeholder=""Recipient's username"" aria-describedby=""basic-addon2"">
    <span class=""input-group-addon"" id=""basic-addon2"">@example.com</span>
    </div>

    <div class=""input-group"">
    <span class=""input-group-addon"">$</span>
    <input type=""text"" class=""form-control"" aria-label=""Amount (to the nearest dollar)"">
    <span class=""input-group-addon"">.00</span>
    </div>

    <label for=""basic-url"">Your vanity URL</label>
    <div class=""input-group"">
    <span class=""input-group-addon"" id=""basic-addon3"">https://example.com/users/</span>
    <input type=""text"" class=""form-control"" id=""basic-url"" aria-describedby=""basic-addon3"">
    </div>
</form>
", "IForm<SomeEnum>", 0)] // input groups

    [InlineData(@"
<form class=""navbar-form navbar-left"" role=""search"">
    <div class=""input-group input-group-lg"">
    <span class=""input-group-addon"" id=""sizing-addon1"">@</span>
    <input type=""text"" class=""form-control"" placeholder=""Username"" aria-describedby=""sizing-addon1"">
    </div>

    <div class=""input-group"">
    <span class=""input-group-addon"" id=""sizing-addon2"">@</span>
    <input type=""text"" class=""form-control"" placeholder=""Username"" aria-describedby=""sizing-addon2"">
    </div>

    <div class=""input-group input-group-sm"">
    <span class=""input-group-addon"" id=""sizing-addon3"">@</span>
    <input type=""text"" class=""form-control"" placeholder=""Username"" aria-describedby=""sizing-addon3"">
    </div>
</form>
", "IForm<SomeEnum>", 0)] // sizing

    [InlineData(@"
<form class=""navbar-form navbar-left"" role=""search"">
    <div class=""row"">
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <span class=""input-group-addon"">
        <input type=""checkbox"" aria-label=""..."">
      </span>
      <input type=""text"" class=""form-control"" aria-label=""..."">
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <span class=""input-group-addon"">
        <input type=""radio"" aria-label=""..."">
      </span>
      <input type=""text"" class=""form-control"" aria-label=""..."">
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    </div><!-- /.row -->
</form>
", "IForm<SomeEnum>", 0)] // Checkboxes and radio addons
    [InlineData(@"
<form class=""navbar-form navbar-left"" role=""search"">
    <div class=""row"">
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <span class=""input-group-btn"">
        <button class=""btn btn-default"" type=""button"">Go!</button>
      </span>
      <input type=""text"" class=""form-control"" placeholder=""Search for..."">
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <input type=""text"" class=""form-control"" placeholder=""Search for..."">
      <span class=""input-group-btn"">
        <button class=""btn btn-default"" type=""button"">Go!</button>
      </span>
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    </div><!-- /.row -->
</form>
", "IForm<SomeEnum>", 0)] // Button addons
    [InlineData(@"
<form class=""navbar-form navbar-left"" role=""search"">
    <div class=""row"">
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <div class=""input-group-btn"">
        <button type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">Action <span class=""caret""></span></button>
        <ul class=""dropdown-menu"">
          <li><a href=""#"">Action</a></li>
          <li><a href=""#"">Another action</a></li>
          <li><a href=""#"">Something else here</a></li>
          <li role=""separator"" class=""divider""></li>
          <li><a href=""#"">Separated link</a></li>
        </ul>
      </div><!-- /btn-group -->
      <input type=""text"" class=""form-control"" aria-label=""..."">
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    <div class=""col-lg-6"">
    <div class=""input-group"">
      <input type=""text"" class=""form-control"" aria-label=""..."">
      <div class=""input-group-btn"">
        <button type=""button"" class=""btn btn-default dropdown-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">Action <span class=""caret""></span></button>
        <ul class=""dropdown-menu dropdown-menu-right"">
          <li><a href=""#"">Action</a></li>
          <li><a href=""#"">Another action</a></li>
          <li><a href=""#"">Something else here</a></li>
          <li role=""separator"" class=""divider""></li>
          <li><a href=""#"">Separated link</a></li>
        </ul>
      </div><!-- /btn-group -->
    </div><!-- /input-group -->
    </div><!-- /.col-lg-6 -->
    </div><!-- /.row -->
</form>
", "IForm<SomeEnum>", 0)] // Buttons with dropdowns
        #region commented
        /*
    [InlineData(@"
<div class="input-group">
<div class="input-group-btn">
<!-- Button and dropdown menu -->
</div>
<input type="text" class="form-control" aria-label="...">
</div>

<div class="input-group">
<input type="text" class="form-control" aria-label="...">
<div class="input-group-btn">
<!-- Button and dropdown menu -->
</div>
</div>
", "")] // Segmented buttons
    [InlineData(@"
<div class="input-group">
<div class="input-group-btn">
<!-- Buttons -->
</div>
<input type="text" class="form-control" aria-label="...">
</div>

<div class="input-group">
<input type="text" class="form-control" aria-label="...">
<div class="input-group-btn">
<!-- Buttons -->
</div>
</div>
", "")] // Multiple buttons
    [InlineData(@"
<ul class="nav nav-tabs">
<li role="presentation" class="active"><a href="#">Home</a></li>
<li role="presentation"><a href="#">Profile</a></li>
<li role="presentation"><a href="#">Messages</a></li>
</ul>
", "")] // Tabs
    [InlineData(@"
<ul class="nav nav-pills">
<li role="presentation" class="active"><a href="#">Home</a></li>
<li role="presentation"><a href="#">Profile</a></li>
<li role="presentation"><a href="#">Messages</a></li>
</ul>
", "")] // Pills
    [InlineData(@"
<ul class="nav nav-pills nav-stacked">
...
</ul>
", "")]
    [InlineData(@"
<ul class="nav nav-tabs nav-justified">
...
</ul>
<ul class="nav nav-pills nav-justified">
...
</ul>
", "")] // Justified
    [InlineData(@"
<ul class="nav nav-pills">
...
<li role="presentation" class="disabled"><a href="#">Disabled link</a></li>
...
</ul>
", "")] // Disabled links
    [InlineData(@"
<ul class="nav nav-tabs">
...
<li role="presentation" class="dropdown">
<a class="dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">
  Dropdown <span class="caret"></span>
</a>
<ul class="dropdown-menu">
  ...
</ul>
</li>
...
</ul>
", "")] // Using dropdowns
    [InlineData(@"
<ul class="nav nav-pills">
...
<li role="presentation" class="dropdown">
<a class="dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">
  Dropdown <span class="caret"></span>
</a>
<ul class="dropdown-menu">
  ...
</ul>
</li>
...
</ul>
", "")] // Pills with dropdowns
    [InlineData(@"
<nav class="navbar navbar-default">
<div class="container-fluid">
<!-- Brand and toggle get grouped for better mobile display -->
<div class="navbar-header">
  <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false">
    <span class="sr-only">Toggle navigation</span>
    <span class="icon-bar"></span>
    <span class="icon-bar"></span>
    <span class="icon-bar"></span>
  </button>
  <a class="navbar-brand" href="#">Brand</a>
</div>

<!-- Collect the nav links, forms, and other content for toggling -->
<div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
  <ul class="nav navbar-nav">
    <li class="active"><a href="#">Link <span class="sr-only">(current)</span></a></li>
    <li><a href="#">Link</a></li>
    <li class="dropdown">
      <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Dropdown <span class="caret"></span></a>
      <ul class="dropdown-menu">
        <li><a href="#">Action</a></li>
        <li><a href="#">Another action</a></li>
        <li><a href="#">Something else here</a></li>
        <li role="separator" class="divider"></li>
        <li><a href="#">Separated link</a></li>
        <li role="separator" class="divider"></li>
        <li><a href="#">One more separated link</a></li>
      </ul>
    </li>
  </ul>
  <form class="navbar-form navbar-left" role="search">
    <div class="form-group">
      <input type="text" class="form-control" placeholder="Search">
    </div>
    <button type="submit" class="btn btn-default">Submit</button>
  </form>
  <ul class="nav navbar-nav navbar-right">
    <li><a href="#">Link</a></li>
    <li class="dropdown">
      <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Dropdown <span class="caret"></span></a>
      <ul class="dropdown-menu">
        <li><a href="#">Action</a></li>
        <li><a href="#">Another action</a></li>
        <li><a href="#">Something else here</a></li>
        <li role="separator" class="divider"></li>
        <li><a href="#">Separated link</a></li>
      </ul>
    </li>
  </ul>
</div><!-- /.navbar-collapse -->
</div><!-- /.container-fluid -->
</nav>
", "")] // Default NavBar
    [InlineData(@"
<nav class="navbar navbar-default">
<div class="container-fluid">
<div class="navbar-header">
  <a class="navbar-brand" href="#">
    <img alt="Brand" src="...">
  </a>
</div>
</div>
</nav>
", "")] // Brand image


    [InlineData(@"
<form class="navbar-form navbar-left" role="search">
<div class="form-group">
<input type="text" class="form-control" placeholder="Search">
</div>
<button type="submit" class="btn btn-default">Submit</button>
</form>
", "")] // Forms
    [InlineData(@"
<nav class="navbar navbar-default navbar-fixed-top">
<div class="container">
...
</div>
</nav>
", "")] // Component alignment // Fixed to top
    [InlineData(@"
<nav class="navbar navbar-default navbar-fixed-bottom">
<div class="container">
...
</div>
</nav>
", "")] // Fixed to bottom
    [InlineData(@"
<nav class="navbar navbar-default navbar-static-top">
<div class="container">
...
</div>
</nav>
", "")] // Static top
    [InlineData(@"
<nav class="navbar navbar-inverse">
...
</nav>
", "")] // Inverted navbar
    [InlineData(@"
<ol class="breadcrumb">
<li><a href="#">Home</a></li>
<li><a href="#">Library</a></li>
<li class="active">Data</li>
</ol>
", "")] // Breadcrumbs
    [InlineData(@"
<nav>
<ul class="pagination">
<li>
  <a href="#" aria-label="Previous">
    <span aria-hidden="true">&laquo;</span>
  </a>
</li>
<li><a href="#">1</a></li>
<li><a href="#">2</a></li>
<li><a href="#">3</a></li>
<li><a href="#">4</a></li>
<li><a href="#">5</a></li>
<li>
  <a href="#" aria-label="Next">
    <span aria-hidden="true">&raquo;</span>
  </a>
</li>
</ul>
</nav>
", "")] // Pagination // Default pagination
    [InlineData(@"
<nav>
<ul class="pagination">
<li class="disabled"><a href="#" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>
<li class="active"><a href="#">1 <span class="sr-only">(current)</span></a></li>
...
</ul>
</nav>
", "")] // Disabled and active states
    [InlineData(@"
<nav>
<ul class="pagination">
<li class="disabled">
  <span>
    <span aria-hidden="true">&laquo;</span>
  </span>
</li>
<li class="active">
  <span>1 <span class="sr-only">(current)</span></span>
</li>
...
</ul>
</nav>
", "")]
    [InlineData(@"
<nav><ul class="pagination pagination-lg">...</ul></nav>
<nav><ul class="pagination">...</ul></nav>
<nav><ul class="pagination pagination-sm">...</ul></nav>
", "")] // Sizing
    [InlineData(@"
<nav>
<ul class="pager">
<li><a href="#">Previous</a></li>
<li><a href="#">Next</a></li>
</ul>
</nav>
", "")] // Pager // Default example

    [InlineData(@"
<nav>
<ul class="pager">
<li class="previous"><a href="#"><span aria-hidden="true">&larr;</span> Older</a></li>
<li class="next"><a href="#">Newer <span aria-hidden="true">&rarr;</span></a></li>
</ul>
</nav>
", "")] // Aligned links
    [InlineData(@"
<nav>
<ul class="pager">
<li class="previous disabled"><a href="#"><span aria-hidden="true">&larr;</span> Older</a></li>
<li class="next"><a href="#">Newer <span aria-hidden="true">&rarr;</span></a></li>
</ul>
</nav>
", "")] // Optional disabled state
    [InlineData(@"
<div class="row">
<div class="col-xs-6 col-md-3">
<a href="#" class="thumbnail">
  <img src="..." alt="...">
</a>
</div>
...
</div>
", "")] // Thumbnails // Default example
    [InlineData(@"
<div class="row">
<div class="col-sm-6 col-md-4">
<div class="thumbnail">
  <img src="..." alt="...">
  <div class="caption">
    <h3>Thumbnail label</h3>
    <p>...</p>
    <p><a href="#" class="btn btn-primary" role="button">Button</a> <a href="#" class="btn btn-default" role="button">Button</a></p>
  </div>
</div>
</div>
</div>
", "")] // Custom content
    [InlineData(@"
<div class="progress">
<div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 60%;">
<span class="sr-only">60% Complete</span>
</div>
</div>
", "")] // Progress bars
    [InlineData(@"
<div class="progress">
<div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 60%;">
60%
</div>
</div>
", "")] // With label
    [InlineData(@"
<div class="progress">
<div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="min-width: 2em;">
0%
</div>
</div>
<div class="progress">
<div class="progress-bar" role="progressbar" aria-valuenow="2" aria-valuemin="0" aria-valuemax="100" style="min-width: 2em; width: 2%;">
2%
</div>
</div>
", "")]
    [InlineData(@"
<div class="progress">
<div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100" style="width: 40%">
<span class="sr-only">40% Complete (success)</span>
</div>
</div>
<div class="progress">
<div class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="20" aria-valuemin="0" aria-valuemax="100" style="width: 20%">
<span class="sr-only">20% Complete</span>
</div>
</div>
<div class="progress">
<div class="progress-bar progress-bar-warning" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 60%">
<span class="sr-only">60% Complete (warning)</span>
</div>
</div>
<div class="progress">
<div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="80" aria-valuemin="0" aria-valuemax="100" style="width: 80%">
<span class="sr-only">80% Complete (danger)</span>
</div>
</div>
", "")] // Contextual alternatives
    [InlineData(@"
<div class="progress">
<div class="progress-bar progress-bar-success progress-bar-striped" role="progressbar" aria-valuenow="40" aria-valuemin="0" aria-valuemax="100" style="width: 40%">
<span class="sr-only">40% Complete (success)</span>
</div>
</div>
<div class="progress">
<div class="progress-bar progress-bar-info progress-bar-striped" role="progressbar" aria-valuenow="20" aria-valuemin="0" aria-valuemax="100" style="width: 20%">
<span class="sr-only">20% Complete</span>
</div>
</div>
<div class="progress">
<div class="progress-bar progress-bar-warning progress-bar-striped" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 60%">
<span class="sr-only">60% Complete (warning)</span>
</div>
</div>
<div class="progress">
<div class="progress-bar progress-bar-danger progress-bar-striped" role="progressbar" aria-valuenow="80" aria-valuemin="0" aria-valuemax="100" style="width: 80%">
<span class="sr-only">80% Complete (danger)</span>
</div>
</div>
", "")] // Striped
    [InlineData(@"
<div class="progress">
<div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="45" aria-valuemin="0" aria-valuemax="100" style="width: 45%">
<span class="sr-only">45% Complete</span>
</div>
</div>
", "")] // Animated
    [InlineData(@"
<div class="progress">
<div class="progress-bar progress-bar-success" style="width: 35%">
<span class="sr-only">35% Complete (success)</span>
</div>
<div class="progress-bar progress-bar-warning progress-bar-striped" style="width: 20%">
<span class="sr-only">20% Complete (warning)</span>
</div>
<div class="progress-bar progress-bar-danger" style="width: 10%">
<span class="sr-only">10% Complete (danger)</span>
</div>
</div>
", "")] // Stacked

    [InlineData(@"
<div class="media">
<div class="media-left">
<a href="#">
  <img class="media-object" src="..." alt="...">
</a>
</div>
<div class="media-body">
<h4 class="media-heading">Media heading</h4>
...
</div>
</div>
", "")] // Media object // Default media
    [InlineData(@"
<div class="media">
<div class="media-left media-middle">
<a href="#">
  <img class="media-object" src="..." alt="...">
</a>
</div>
<div class="media-body">
<h4 class="media-heading">Middle aligned media</h4>
...
</div>
</div>
", "")] // Media alignment
    [InlineData(@"
<ul class="media-list">
<li class="media">
<div class="media-left">
  <a href="#">
    <img class="media-object" src="..." alt="...">
  </a>
</div>
<div class="media-body">
  <h4 class="media-heading">Media heading</h4>
  ...
</div>
</li>
</ul>
", "")] // Media list
    [InlineData(@"
<ul class="list-group">
<li class="list-group-item">Cras justo odio</li>
<li class="list-group-item">Dapibus ac facilisis in</li>
<li class="list-group-item">Morbi leo risus</li>
<li class="list-group-item">Porta ac consectetur ac</li>
<li class="list-group-item">Vestibulum at eros</li>
</ul>
", "")] // List group
    [InlineData(@"
<ul class="list-group">
<li class="list-group-item">
<span class="badge">14</span>
Cras justo odio
</li>
</ul>
", "")] // Badges
    [InlineData(@"
<div class="list-group">
<a href="#" class="list-group-item active">
Cras justo odio
</a>
<a href="#" class="list-group-item">Dapibus ac facilisis in</a>
<a href="#" class="list-group-item">Morbi leo risus</a>
<a href="#" class="list-group-item">Porta ac consectetur ac</a>
<a href="#" class="list-group-item">Vestibulum at eros</a>
</div>
", "")] // Linked items
    [InlineData(@"
<div class="list-group">
<button type="button" class="list-group-item">Cras justo odio</button>
<button type="button" class="list-group-item">Dapibus ac facilisis in</button>
<button type="button" class="list-group-item">Morbi leo risus</button>
<button type="button" class="list-group-item">Porta ac consectetur ac</button>
<button type="button" class="list-group-item">Vestibulum at eros</button>
</div>
", "")] // Button items
    [InlineData(@"
<div class="list-group">
<a href="#" class="list-group-item disabled">
Cras justo odio
</a>
<a href="#" class="list-group-item">Dapibus ac facilisis in</a>
<a href="#" class="list-group-item">Morbi leo risus</a>
<a href="#" class="list-group-item">Porta ac consectetur ac</a>
<a href="#" class="list-group-item">Vestibulum at eros</a>
</div>
", "")] // Disabled items
    [InlineData(@"
<ul class="list-group">
<li class="list-group-item list-group-item-success">Dapibus ac facilisis in</li>
<li class="list-group-item list-group-item-info">Cras sit amet nibh libero</li>
<li class="list-group-item list-group-item-warning">Porta ac consectetur ac</li>
<li class="list-group-item list-group-item-danger">Vestibulum at eros</li>
</ul>
<div class="list-group">
<a href="#" class="list-group-item list-group-item-success">Dapibus ac facilisis in</a>
<a href="#" class="list-group-item list-group-item-info">Cras sit amet nibh libero</a>
<a href="#" class="list-group-item list-group-item-warning">Porta ac consectetur ac</a>
<a href="#" class="list-group-item list-group-item-danger">Vestibulum at eros</a>
</div>
", "")] // Contextual classes
    [InlineData(@"
<div class="list-group">
<a href="#" class="list-group-item active">
<h4 class="list-group-item-heading">List group item heading</h4>
<p class="list-group-item-text">...</p>
</a>
</div>
", "")] // Custom content
    [InlineData(@"
<div class="panel panel-default">
<div class="panel-body">
Basic panel example
</div>
</div>
", "")] // Panels

    [InlineData(@"
<div class="panel panel-default">
<div class="panel-heading">Panel heading without title</div>
<div class="panel-body">
Panel content
</div>
</div>

<div class="panel panel-default">
<div class="panel-heading">
<h3 class="panel-title">Panel title</h3>
</div>
<div class="panel-body">
Panel content
</div>
</div>
", "")] // Panel with heading
    [InlineData(@"
<div class="panel panel-default">
<div class="panel-body">
Panel content
</div>
<div class="panel-footer">Panel footer</div>
</div>
", "")] // Panel with footer
    [InlineData(@"
<div class="panel panel-primary">...</div>
<div class="panel panel-success">...</div>
<div class="panel panel-info">...</div>
<div class="panel panel-warning">...</div>
<div class="panel panel-danger">...</div>
", "")] // Contextual alternatives
    [InlineData(@"
<div class="panel panel-default">
<!-- Default panel contents -->
<div class="panel-heading">Panel heading</div>
<div class="panel-body">
<p>...</p>
</div>

<!-- Table -->
<table class="table">
...
</table>
</div>
", "")] // With tables
    [InlineData(@"
<div class="panel panel-default">
<!-- Default panel contents -->
<div class="panel-heading">Panel heading</div>

<!-- Table -->
<table class="table">
...
</table>
</div>
", "")]
    [InlineData(@"
<div class="panel panel-default">
<!-- Default panel contents -->
<div class="panel-heading">Panel heading</div>
<div class="panel-body">
<p>...</p>
</div>

<!-- List group -->
<ul class="list-group">
<li class="list-group-item">Cras justo odio</li>
<li class="list-group-item">Dapibus ac facilisis in</li>
<li class="list-group-item">Morbi leo risus</li>
<li class="list-group-item">Porta ac consectetur ac</li>
<li class="list-group-item">Vestibulum at eros</li>
</ul>
</div>
", "")] // With list groups
    [InlineData(@"
<!-- 16:9 aspect ratio -->
<div class="embed-responsive embed-responsive-16by9">
<iframe class="embed-responsive-item" src="..."></iframe>
</div>

<!-- 4:3 aspect ratio -->
<div class="embed-responsive embed-responsive-4by3">
<iframe class="embed-responsive-item" src="..."></iframe>
</div>
", "")] // Responsive embed
    [InlineData(@"

", "")]

    [InlineData(@"

", "")]
    [InlineData(@"

", "")]
    [InlineData(@"

", "")]
    [InlineData(@"

", "")]
    [InlineData(@"

", "")]
    [InlineData(@"

", "")]
    [InlineData(@"

", "")]
    [InlineData(@"

", "")]
    [InlineData(@"

", "")]
    [InlineData(@"

", "")]
    [InlineData(@"

", "")]
*/
        #endregion
        [Trait("Category", "Bootstrap 3, collection")]
        public void ParseBootstrap3ForCollection(string input, string expected, int elementPosition)
        {
            GivenHtml(input);
            WhenParsing(elementPosition);
            ThenThereIsCollectionOfElementsOfType(expected);
        }
        
        void GivenHtml(string input)
        {
            var fullHtml = @"<html><head></head><body>" + input + "</body></html>";
            _doc = new HtmlDocument();
            _doc.LoadHtml(fullHtml);
        }

        void WhenParsing(int elementPosition)
        {
            var pageLoader = new PageLoader();
            _entries.AddRange(pageLoader.GetCodeEntries(_doc.DocumentNode, TestFactory.ExcludeList));
            _entry = _entries.Cast<CodeEntry>().ToArray()[elementPosition];
        }

        void ThenThereIsElementOfType(string expected)
        {
            Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expected));
        }

        void ThenThereIsCollectionOfElementsOfType(string expected)
        {
            //Console.WriteLine("================================================================================================");
            //Console.WriteLine(_entry.GenerateCodeForEntry(SupportedLanguages.Java));

            Assert.True(_entry.GenerateCodeForEntry(SupportedLanguages.Java).Contains(expected));
        }
    }
}