import React from "react";
import FileInput from "./fileInput";
import PersonList from "./personList";

export default React.createClass({
getInitialState: function () {
   return { data: []};
},
componentWillMount: function () {
   
},
componentDidMount: function() {
 
},
processFile: function() {
    var reactMain = this;
    var xhr = new XMLHttpRequest();
    var fd = new FormData();
    fd.append("file", this.refs.file.refs.input.files[0]);
    xhr.open("POST", "/WageCalculator/ProcessCsvFile/", true);
    xhr.send(fd);
    xhr.addEventListener("load", function(event) {
        var data = JSON.parse(event.target.response);
        reactMain.setState({ data: data });
    }, false);
},
render: function () {
    return (
    <div id="main">
        <h1>Hello there and welcome to the wage calculator!</h1>
        <div><FileInput ref="file" id="fileinput" onChange={this.processFile}/></div>
        <PersonList data={this.state.data} />
        <div className="background-image-text">
            <div>
                Happy New Year!
            </div>
        </div>
    </div>
);
}
});

