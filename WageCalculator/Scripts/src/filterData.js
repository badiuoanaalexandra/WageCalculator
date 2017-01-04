import React from "react";
import moment from "moment";

export default React.createClass({
    componentWillMount:function() {
        //default values
        this.selectedMonth = 1;
        this.selectedYear = 2014;
        this.selectedPerson = 0;
    },
    getMonthsData: function() {
        var monthValues = [];
        for (var i = 0; i < this.props.filterData.Months.length; i++) {
            monthValues.push({
                value:this.props.filterData.Months[i],
                label: moment(this.props.filterData.Months[i], 'M').format('MMMM')
            });
        }
        return monthValues;
    },
    setSelectedMonth: function(e) {
        this.selectedMonth = e.target.value;
    },
    setSelectedYear: function(e) {
        this.selectedYear = e.target.value;
    },
    setSelectedPerson: function(e) {
        this.selectedPerson = e.target.value;
    },
    render: function () {
        if (this.props.filterData == null) {
            return null;
        }

        var monthsOptions = this.getMonthsData();
        var monthsAvailable = monthsOptions.map(function(option) {
                return (
                   <option value={option.value} key={option.value}>{option.label}</option>
                    );
         });
        
         if (monthsAvailable.length > 0) {
                 this.selectedMonth = monthsOptions[0].value;
         }

         var yearsAvailable = this.props.filterData.Years.map(function(year) {
                return (
                   <option value={year} key={year}>{year}</option>
                );
         });

         if (this.props.filterData.Years.length > 0) {
                this.selectedYear = this.props.filterData.Years[0];
         }

         var personListItems= this.props.filterData.PersonListItems.map(function(person) {
                return (
                   <option value={person.PersonID} key={person.PersonID}>{person.PersonName}</option>
                );
         });

        return (
          <div className="filter">
            <div>
               <div>Select Month</div>
               <select onChange={this.setSelectedMonth}>
                   {monthsAvailable}
                </select>
            </div>
            <div>
                <div>Select year</div>
                <select onChange={this.setSelectedYear}>
                   {yearsAvailable}
                 </select>
            </div>
            <div>
                <div>Select person</div>
                <select onChange={this.setSelectedPerson}>
                   <option value={0} key={0}>All</option>
                   {personListItems}
                </select>
            </div>
                <div className="button-wrapper">
                    <a className="button" onClick={this.props.calculateFunction}>Calculate</a>
                    <img ref="loadingFilter" src="/Content/Images/loading.svg"/>
                </div>
               
          </div>  
        );
        }
});

