namespace JdiCodeGenerator.Tests.Recognition
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using HtmlAgilityPack;
    using Internals;
    using Xunit;
    using Web.Helpers;
    using Web.ObjectModel.Abstract;

    public class Bootstrap3
    {
        CodeEntry<HtmlElementTypes> _entry;
        HtmlDocument _doc;
        readonly List<ICodeEntry<HtmlElementTypes>> _entries;

        const string HtmlFirstPart = @"
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""utf-8"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>Bootstrap 101 Template</title>

    <!-- Bootstrap -->
    <link href=""css/bootstrap.min.css"" rel=""stylesheet"">

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src=""https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js""></script>
      <script src=""https://oss.maxcdn.com/respond/1.4.2/respond.min.js""></script>
    <![endif]-->
  </head>
  <body>
";
        const string HtmlLastPart = @"
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src=""https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js""></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src=""js/bootstrap.min.js""></script>
  </body>
</html>
";

        public Bootstrap3()
        {
            _entry = null;
            _doc = null;
            _entries = new List<ICodeEntry<HtmlElementTypes>>();
        }

        [Theory]
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonDefault.txt", "IButton", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonDefaultLarge.txt", "IButton", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\Alert.txt", "IText", 0)] // alert
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonDefaultNavBar.txt", "IButton", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\NavBarText.txt", "IText", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\NavBarTextNavBarRight.txt", "IText", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelDefaultHeading.txt", "ILabel", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelDefault.txt", "ILabel", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelPrimary.txt", "ILabel", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelSuccess.txt", "ILabel", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelInfo.txt", "ILabel", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelWarning.txt", "ILabel", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelDanger.txt", "ILabel", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\Badge.txt", "ILink", 0)] // Badges
        [InlineData(@"..\Data\Bootstrap3\Simple\TabList.txt", "ITabs", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\Jumbotron.txt", "IText", 0)] // Jumbotron
        [InlineData(@"..\Data\Bootstrap3\Simple\Jumbotron2.txt", "IText", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\PageHeader.txt", "IText", 0)] // Page header
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertSuccess.txt", "IText", 0)] // Alerts
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertInfo.txt", "IText", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertWarning.txt", "IText", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertDanger.txt", "IText", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertDismissible.txt", "IText", 0)] // Dismissible alerts
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertSuccessLink.txt", "ILink", 1)] // Links in alerts
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertInfoLink.txt", "ILink", 1)]
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertWarningLink.txt", "ILink", 1)]
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertDangerLink.txt", "ILink", 1)]
        [InlineData(@"..\Data\Bootstrap3\Simple\Well.txt", "IText", 0)] // Wells // Default well
        [InlineData(@"..\Data\Bootstrap3\Simple\WellLarge.txt", "IText", 0)] // Optional classes
        [InlineData(@"..\Data\Bootstrap3\Simple\WellSmall.txt", "IText", 0)]
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroup0.txt", "IButton", 0)] // button groups
                  // ", "IButton", 1)] // button groups
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroup1.txt", "IButton", 1)] // button groups
                  // ", "IButton", 2)] // button groups
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroup2.txt", "IButton", 2)] // button groups
                  // ", "IButton", 3)] // button groups
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonToolbar0.txt", "IButton", 0)] // button toolbar
                  // ", "IButton", 2)] // button toolbar
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonToolbar1.txt", "IButton", 0)] // button toolbar
                  // ", "IButton", 3)] // button toolbar
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonToolbar2.txt", "IButton", 0)] // button toolbar
                  // ", "IButton", 4)] // button toolbar
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroupSizing0.txt", "IButton", 0)] // sizing
                  // ", "IButton", 1)] // sizing
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroupSizing1.txt", "IButton", 0)] // sizing
                  // ", "IButton", 1)] // sizing
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroupSizing2.txt", "IButton", 0)] // sizing
                  // ", "IButton", 1)] // sizing
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroupSizing3.txt", "IButton", 0)] // sizing
                  // ", "IButton", 1)] // sizing
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
        [InlineData(@"..\Data\Bootstrap3\Simple\InputGroupAddon0.txt", "ITextField", 0)]
        // ", "ITextField", 2)]
        [InlineData(@"..\Data\Bootstrap3\Simple\InputGroupAddon1.txt", "ITextField", 0)]
        // ", "ITextField", 1)]
        [InlineData(@"..\Data\Bootstrap3\Simple\InputGroupAddon2.txt", "ITextField", 0)]
        // ", "ITextField", 2)]
        [InlineData(@"..\Data\Bootstrap3\Simple\TextField.txt", "ITextField", 1)]
        // ", "ITextField", 3)]

        [InlineData(@"..\Data\Bootstrap3\Simple\CheckboxesAndRadioAddons0.txt", "ICheckBox", 1)] // Checkboxes and radio addons
                    // ", "ICheckBox", 5)] // Checkboxes and radio addons
        // [InlineData(@"..\Data\Bootstrap3\Simple\CheckboxesAndRadioAddons1.txt", "IRadioButtons", 10)] // Checkboxes and radio addons
        [InlineData(@"..\Data\Bootstrap3\Simple\CheckboxesAndRadioAddons1.txt", "IRadioButtons", 3)] // Checkboxes and radio addons
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonAddons.txt", "IButton", 4)] // Button addons
                  // ", "IButton", 11)] // Button addons
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonsWithDropdowns.txt", "IButton", 1)] // Buttons with dropdowns
        [Trait("Category", "Bootstrap 3, single element")]
        public void ParseBootstrap3ForSingleElementNew(string input, string expected, int elementPosition)
        {
            GivenHtml_NewHtmlInFiles(input);
            WhenParsing(elementPosition);
            ThenThereIsElementOfType(expected);
        }

        //[Theory]
        //[Trait("Category", "OLD Bootstrap 3, single element")]
        //public void ParseBootstrap3ForSingleElementOld(string input, string expected, int elementPosition)
        //{
        //    GivenHtml_OriginalForHtmlInCode(input);
        //    WhenParsing(elementPosition);
        //    ThenThereIsElementOfType(expected);
        //}

        [Theory]
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDown.txt", "IDropDown<SomeEnum>", 0)]
        [InlineData(@"..\Data\Bootstrap3\Complex\DropUp.txt", "IDropDown<SomeEnum>", 0)]
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownAlignment.txt", "IDropDown<SomeEnum>", 0)] // alignment
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownHeaders.txt", "IDropDown<SomeEnum>", 0)] // headers
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownDivider.txt", "IMenuItem", 0)] // divider
                    // ", "IElement", 2)] // divider
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownDisabledMenuItems.txt", "ILink", 3)] // disabled menu items
        [InlineData(@"..\Data\Bootstrap3\Complex\ButtonGroupNesting.txt", "IDropDown<SomeEnum>", 0)] // nesting
                              // 20160610
                              // ", "IDropDown<SomeEnum>", 3)] // nesting
        [InlineData(@"..\Data\Bootstrap3\Complex\ButtonGroupVerticalVariation.txt", "IDropDown<SomeEnum>", 0)] // vertical variation
                              // 20160610
                              // ", "IDropDown<SomeEnum>", 4)] // vertical variation

        [InlineData(@"..\Data\Bootstrap3\Complex\JustifiedButtonGroup.txt", "IDropDown<SomeEnum>", 3)] // justified button groups
                              // 20160610
                              // ", "IDropDown<SomeEnum>", 8)] // justified button groups

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
        [InlineData(@"..\Data\Bootstrap3\Complex\ButtonDropDown.txt", "IDropDown<SomeEnum>", 0)] // button dropdowns
        [InlineData(@"..\Data\Bootstrap3\Complex\SplitButtonDropDown.txt", "IDropDown<SomeEnum>", 0)] // split button dropdowns
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownMenuLargeButtonGroup.txt", "IDropDown<SomeEnum>", 0)]
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownMenuSmallButtonGroup.txt", "IDropDown<SomeEnum>", 0)]
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownMenuExtraSmallButtonGroup.txt", "IDropDown<SomeEnum>", 0)] // sizing x3
        [InlineData(@"..\Data\Bootstrap3\Complex\DropUpVariation.txt", "IDropDown<SomeEnum>", 0)] // dropup variation
        [InlineData(@"..\Data\Bootstrap3\Complex\InputGroups.txt", "IForm<SomeEnum>", 0)] // input groups
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormSizing.txt", "IForm<SomeEnum>", 0)] // sizing
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormCheckboxesAndRadioButtons.txt", "IForm<SomeEnum>", 0)] // Checkboxes and radio addons
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormButtonAddons.txt", "IForm<SomeEnum>", 0)] // Button addons
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormButtonsWithDropDowns.txt", "IForm<SomeEnum>", 0)] // Buttons with dropdowns
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormSegmentedButtons.txt", "IForm<SomeEnum>", 0)] // Segmented buttons
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormMultipleButtons.txt", "IForm<SomeEnum>", 0)] // Multiple buttons
        #region commented
        /*
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
*/
        #endregion
        [InlineData(@"..\Data\Bootstrap3\Complex\DefaultNavBar.txt", "INavBar", 0)] // Default NavBar
        #region commented
        /*
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
*/
        #endregion
        [Trait("Category", "Bootstrap 3, collection")]
        public void ParseBootstrap3ForCollectionNew(string input, string expected, int elementPosition)
        {
            GivenHtml_NewHtmlInFiles(input);
            WhenParsing(elementPosition);
            ThenThereIsCollectionOfElementsOfType(expected);
        }

        //[Theory]
        //[Trait("Category", "OLD Bootstrap 3, collection")]
        //public void ParseBootstrap3ForCollectionOld(string input, string expected, int elementPosition)
        //{
        //    GivenHtml_OriginalForHtmlInCode(input);
        //    WhenParsing(elementPosition);
        //    ThenThereIsCollectionOfElementsOfType(expected);
        //}
        
        //void GivenHtml_OriginalForHtmlInCode(string input)
        //{
        //    // var fullHtml = @"<html><head></head><body>" + input + "</body></html>";
        //    var fullHtml = HtmlFirstPart + input + HtmlLastPart;
        //    _doc = new HtmlDocument();
        //    _doc.LoadHtml(fullHtml);
        //}

        void GivenHtml_NewHtmlInFiles(string path)
        {
            // var fullHtml = @"<html><head></head><body>" + input + "</body></html>";
            var fullHtml = string.Empty;
#if DEBUG
            path = @"Debug\" + path;
#else
            path = @"Release\" + path;
#endif
            using (var reader = new StreamReader(path))
            {
                fullHtml = HtmlFirstPart + reader.ReadToEnd() + HtmlLastPart;
                reader.Close();
            }
            // var fullHtml = HtmlFirstPart + path + HtmlLastPart;
            _doc = new HtmlDocument();
            _doc.LoadHtml(fullHtml);
        }

        void WhenParsing(int elementPosition)
        {
            var pageLoader = new PageLoader();
            _entries.AddRange(pageLoader.GetCodeEntriesFromNode<HtmlElementTypes>(_doc.DocumentNode, TestFactory.ExcludeList));
            _entry = _entries.Cast<CodeEntry<HtmlElementTypes>>().ToArray()[elementPosition];
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