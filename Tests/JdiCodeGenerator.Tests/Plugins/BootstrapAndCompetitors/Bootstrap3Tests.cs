namespace JdiCodeGenerator.Tests.Plugins.BootstrapAndCompetitors
{
    using Core.Helpers;
    using Core.ObjectModel;
    using Core.ObjectModel.Abstract;
    using Core.ObjectModel.Enums;
    using HtmlAgilityPack;
    using Web.ObjectModel.Abstract;
    using Web.ObjectModel.Plugins.BootstrapAndCompetitors;
    using Xunit;

    public class Bootstrap3Tests
    {
        PageMemberCodeEntry<HtmlElementTypes> _entry;
        HtmlDocument _doc;

        public Bootstrap3Tests()
        {
            _entry = null;
            _doc = null;
        }

        [Theory]
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonDefault.txt", "IButton")]
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonDefaultLarge.txt", "IButton")]
        [InlineData(@"..\Data\Bootstrap3\Simple\Alert.txt", "IText")] // alert
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonDefaultNavBar.txt", "IButton")]
        [InlineData(@"..\Data\Bootstrap3\Simple\NavBarText.txt", "IText")]
        [InlineData(@"..\Data\Bootstrap3\Simple\NavBarTextNavBarRight.txt", "IText")]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelDefaultHeading.txt", "ILabel")]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelDefault.txt", "ILabel")]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelPrimary.txt", "ILabel")]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelSuccess.txt", "ILabel")]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelInfo.txt", "ILabel")]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelWarning.txt", "ILabel")]
        [InlineData(@"..\Data\Bootstrap3\Simple\LabelDanger.txt", "ILabel")]
        [InlineData(@"..\Data\Bootstrap3\Simple\Badge.txt", "ILink")] // Badges
        [InlineData(@"..\Data\Bootstrap3\Simple\TabList.txt", "ITabs")]
        [InlineData(@"..\Data\Bootstrap3\Simple\Jumbotron.txt", "IText")] // Jumbotron
        [InlineData(@"..\Data\Bootstrap3\Simple\Jumbotron2.txt", "IText")]
        [InlineData(@"..\Data\Bootstrap3\Simple\PageHeader.txt", "IText")] // Page header
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertSuccess.txt", "IText")] // Alerts
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertInfo.txt", "IText")]
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertWarning.txt", "IText")]
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertDanger.txt", "IText")]
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertDismissible.txt", "IText")] // Dismissible alerts
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertSuccessLink.txt", "ILink")] // Links in alerts
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertInfoLink.txt", "ILink")]
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertWarningLink.txt", "ILink")]
        [InlineData(@"..\Data\Bootstrap3\Simple\AlertDangerLink.txt", "ILink")]
        [InlineData(@"..\Data\Bootstrap3\Simple\Well.txt", "IText")] // Wells // Default well
        [InlineData(@"..\Data\Bootstrap3\Simple\WellLarge.txt", "IText")] // Optional classes
        [InlineData(@"..\Data\Bootstrap3\Simple\WellSmall.txt", "IText")]
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonToolbar0.txt", "IButton")] // button toolbar
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonToolbar1.txt", "IButton")] // button toolbar
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonToolbar2.txt", "IButton")] // button toolbar
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroupSizing0.txt", "IButton")] // sizing
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroupSizing1.txt", "IButton")] // sizing
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroupSizing2.txt", "IButton")] // sizing
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroupSizing3.txt", "IButton")] // sizing
        #region commented
        /*
        [InlineData(@"

        ", "")]
        */
        #endregion
        [InlineData(@"..\Data\Bootstrap3\Simple\InputGroupAddon0.txt", "ITextField")]
        [InlineData(@"..\Data\Bootstrap3\Simple\InputGroupAddon1.txt", "ITextField")]
        [InlineData(@"..\Data\Bootstrap3\Simple\InputGroupAddon2.txt", "ITextField")]
        [InlineData(@"..\Data\Bootstrap3\Simple\TextField.txt", "ITextField")]

        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonAddons.txt", "IButton")] // Button addons
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonsWithDropdowns.txt", "IButton")] // Buttons with dropdowns
        [Trait("Category", "Bootstrap 3, single element")]
        public void ParseBootstrap3ForSingleElement(string input, string expectedType)
        {
            GivenHtmlFromFile(input);
            WhenParsing(expectedType);
            ThenThereIsElementOfType(expectedType);
        }

        [Theory]
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroup0.txt", "IButton", 0)] // button groups
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroup1.txt", "IButton", 1)] // button groups
        [InlineData(@"..\Data\Bootstrap3\Simple\ButtonGroup2.txt", "IButton", 2)] // button groups
        [InlineData(@"..\Data\Bootstrap3\Simple\CheckboxesAndRadioAddons0.txt", "ICheckBox", 1)] // Checkboxes and radio addons
        // [InlineData(@"..\Data\Bootstrap3\Simple\CheckboxesAndRadioAddons1.txt", "IRadioButtons", 10)] // Checkboxes and radio addons
        [InlineData(@"..\Data\Bootstrap3\Simple\CheckboxesAndRadioAddons1.txt", "IRadioButtons", 3)] // Checkboxes and radio addons
        [Trait("Category", "Bootstrap 3, single element, positional")]
        public void ParseBootstrap3ForSingleElementWithPosition(string input, string expectedType, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsElementOfType(expectedType);
        }

        [Theory]
        /*
    @JDropdown(
        root = @FindBy(className = "country-selection"),
        value = @FindBy(css = ".country-wrapper .arrow"),
        elementByName = @FindBy(xpath = "*root*//*[contains(@id,'select-box-applicantCountry')]//li[.='%s']"))
    IDropDown country;

    @JDropdown(
            root = @FindBy(className = "city-selection"),
            expand = @FindBy(css = ".city-wrapper .arrow"),
            list = @FindBy(xpath = "*root*//*[contains(@id,'select-box-applicantCity')]//li")
    )
    IDropDown city;
        */
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDown.txt", "IDropDown<", "dropdown", "dropdown-toggle", "dropdown-menu")]
        [InlineData(@"..\Data\Bootstrap3\Complex\DropUp.txt", "IDropDown<", "dropup", "dropdown-toggle", "dropdown-menu")]
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownAlignment.txt", "IDropDown<", "dropdown", "dropdown-toggle", "dropdown-menu")] // alignment
        // TODO: try to use headers?
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownHeaders.txt", "IDropDown<", "dropdown", "dropdown-toggle", "dropdown-menu")] // headers

        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownDivider.txt", "IMenuItem", "", "", "")] // divider
        // [InlineData(@"..\Data\Bootstrap3\Complex\DropDownDisabledMenuItems.txt", "ILink", "", "", "", 3)] // disabled menu items
        [InlineData(@"..\Data\Bootstrap3\Complex\ButtonGroupNesting.txt", "IDropDown<", "btn-group", "dropdown-toggle", "dropdown-menu")] // nesting
        // TODO: try to use vertical?
        [InlineData(@"..\Data\Bootstrap3\Complex\ButtonGroupVerticalVariation.txt", "IDropDown<", "btn-group", "dropdown-toggle", "dropdown-menu")] // vertical variation

        // TODO: remove commenting in the html file back!
        [InlineData(@"..\Data\Bootstrap3\Complex\JustifiedButtonGroup.txt", "IDropDown<", "btn-group", "dropdown-toggle", "dropdown-menu")] // justified button groups

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
        [InlineData(@"..\Data\Bootstrap3\Complex\ButtonDropDown.txt", "IDropDown<", "btn-group", "dropdown-toggle", "dropdown-menu")] // button dropdowns
        [InlineData(@"..\Data\Bootstrap3\Complex\SplitButtonDropDown.txt", "IDropDown<", "btn-group", "dropdown-toggle", "dropdown-menu")] // split button dropdowns
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownMenuLargeButtonGroup.txt", "IDropDown<", "btn-group", "dropdown-toggle", "dropdown-menu")]
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownMenuSmallButtonGroup.txt", "IDropDown<", "btn-group", "dropdown-toggle", "dropdown-menu")]
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownMenuExtraSmallButtonGroup.txt", "IDropDown<", "btn-group", "dropdown-toggle", "dropdown-menu")] // sizing x3
        [InlineData(@"..\Data\Bootstrap3\Complex\DropUpVariation.txt", "IDropDown<", "btn-group", "dropdown-toggle", "dropdown-menu")] // dropup variation
        [InlineData(@"..\Data\Bootstrap3\Complex\InputGroups.txt", "IForm<", "", "", "")] // input groups
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormSizing.txt", "IForm<", "", "", "")] // sizing
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormCheckboxesAndRadioButtons.txt", "IForm<", "", "", "")] // Checkboxes and radio addons
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormButtonAddons.txt", "IForm<", "", "", "")] // Button addons
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormButtonsWithDropDowns.txt", "IForm<", "", "", "")] // Buttons with dropdowns
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormSegmentedButtons.txt", "IForm<", "", "", "")] // Segmented buttons
        [InlineData(@"..\Data\Bootstrap3\Complex\NavBarFormMultipleButtons.txt", "IForm<", "", "", "")] // Multiple buttons
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
        [InlineData(@"..\Data\Bootstrap3\Complex\DefaultNavBar.txt", "INavBar", "", "", "")] // Default NavBar
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

        // [InlineData(@"..\Data\Complex\MetalsColors.txt", "ICheckList<", "", "", "//label", 1)]

        [Trait("Category", "Bootstrap 3, collection")]

        public void ParseBootstrap3ForCollection(string input, string expectedTypeName, string rootSearchString, string valueSearchString, string listSearchString)
        {
            GivenHtmlFromFile(input);
            WhenParsing(expectedTypeName);
            ThenThereIsCollectionOfElementsOfType(expectedTypeName, rootSearchString, valueSearchString, listSearchString);
        }

        [Theory]
        [InlineData(@"..\Data\Bootstrap3\Complex\DropDownDisabledMenuItems.txt", "ILink", "", "", "", 3)] // disabled menu items
        [Trait("Category", "Bootstrap 3, collection, positional")]
        public void ParseBootstrap3ForCollectionWithPosition(string input, string expectedTypeName, string rootSearchString, string valueSearchString, string listSearchString, int elementPosition)
        {
            GivenHtmlFromFile(input);
            WhenParsing(elementPosition);
            ThenThereIsCollectionOfElementsOfType(expectedTypeName, rootSearchString, valueSearchString, listSearchString);
        }

        void GivenHtmlFromFile(string path)
        {
            _doc = new HtmlDocument();
            _doc.LoadHtml(TestFactory.Instance.GetBootstrap3Page(path));
        }

        void WhenParsing(string expectedTypeName)
        {
            _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] { typeof(Bootstrap3) }, expectedTypeName);
        }

        void WhenParsing(int elementPosition)
        {
            _entry = TestFactory.Instance.GetEntryExpected(_doc, new[] { typeof(Bootstrap3) }, elementPosition);
        }

        void ThenThereIsElementOfType(string expected)
        {
            Assert.True(_entry.GenerateCode(SupportedLanguages.Java).Contains(expected));
        }

        void ThenThereIsCollectionOfElementsOfType(string expected, string rootSearchString, string valueSearchString, string listSearchString)
        {
            var generatedCodeEntry = _entry.GenerateCode(SupportedLanguages.Java);
            Assert.True(generatedCodeEntry.Contains(expected));
            if (_entry.JdiMemberType.IsComplexControl())
            {
                if (!string.IsNullOrEmpty(rootSearchString))
                    Assert.False(string.IsNullOrEmpty(_entry.Root.SearchString));
                if (!string.IsNullOrEmpty(valueSearchString))
                    Assert.False(string.IsNullOrEmpty(_entry.Value.SearchString));
                if (!string.IsNullOrEmpty(listSearchString))
                    Assert.False(string.IsNullOrEmpty(_entry.List.SearchString));
            }
        }
    }
}