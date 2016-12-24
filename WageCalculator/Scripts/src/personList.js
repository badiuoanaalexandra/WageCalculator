import React from "react";
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
          {persons}
        </div>
    );
}
});

