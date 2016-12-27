import React from "react";

export default React.createClass({
render: function () {
    return (
      <div className={this.props.className}>
             <input accept=".csv" type="file" id={this.props.id} ref="input" onChange={this.props.onChange} />
      </div>  
);
}
});

