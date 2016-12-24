import React from "react";

export default React.createClass({
componentWillMount: function () {
   
},
componentDidMount: function() {
 
},
render: function () {
    return (
      <div className={this.props.className}>
             <input accept=".csv" type="file" id={this.props.id} ref="input" onChange={this.props.onChange} />
      </div>  
);
}
});

