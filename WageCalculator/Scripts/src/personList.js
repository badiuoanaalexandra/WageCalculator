import React from "react";
import moment from "moment";
import Person from "./person";

export default React.createClass({
    render: function() {
        var persons = this.props.data.map(function(person) {
            return (
               <Person key={person.PersonID} person={person} />
            );
        });

        return (
            <div className="person-list">
                <div className="table-titles">
                    <div>Name</div>
                    <div>Wage</div>
                </div>
                 {persons}
            </div>
            );
}
});

