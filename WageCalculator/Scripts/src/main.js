import React from "react";
import FileInput from "./fileInput";
import PersonList from "./personList";
import FilterData from "./filterData";

export default React.createClass({
    getInitialState: function () {
        return { data: [], filterData:null};
    },
    calculateWage:function() {
        var reactMain = this;
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/WageCalculator/CalculateWage/', true);
        xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
        xhr.onload = function (event) {
            var data = JSON.parse(event.target.response);
            reactMain.setState({data: data, filterData: reactMain.state.filterData});
        };
        xhr.send('month=' + this.refs.filter.selectedMonth + '&year=' + this.refs.filter.selectedYear + '&personId=' + this.refs.filter.selectedPerson);
    },
    processFile: function() {
        if (this.refs.file.refs.input.files.length === 0) {
            this.refs.file.refs.text.innerHTML = "Upload a.csv file";
            this.setState({data: [], filterData: null });
            return;
        }
        this.refs.error.style.display = "none";
        this.refs.file.refs.text.innerHTML = this.refs.file.refs.input.files[0].name;
        var reactMain = this;
        var xhr = new XMLHttpRequest();
        var fd = new FormData();
        fd.append("file", this.refs.file.refs.input.files[0]);
        xhr.open("POST", "/WageCalculator/ProcessCsvFile/", true);
        xhr.send(fd);
        xhr.addEventListener("load", function(event) {
            var filterData = JSON.parse(event.target.response);
            if (filterData.Months) {
                reactMain.setState({ data: [], filterData: filterData });
            } else {
                reactMain.refs.error.innerHTML = "The file you uploaded is not valid. Please upload a different file";
                reactMain.refs.error.style.display = "block";
            }
        }, false);
    },
    render: function () {
        return (
        <div id="main">
            <h1>Hello there and welcome to the wage calculator!</h1>
            <FileInput className="file-upload" ref="file" id="fileinput" onChange={this.processFile}/>
            <div ref="error" className="error" style={{display:"none"}}></div>
            <FilterData ref="filter" filterData={this.state.filterData} calculateFunction={this.calculateWage}/>
            <PersonList data={this.state.data}/>
            <div className="background-image-text">
                <div>
                    Happy New Year!
                </div>
            </div>
        </div>
        );
    }
});

