const validate = require("validate.js");

exports = async function({ query, headers, body}, response) {

    let snapshot = JSON.parse(body.text());
    
    validate.extend(validate.validators.datetime, {
      "parse": function(value, options) {
        return new Date(value).valueOf();
      },
      "format": function(value, options) {
        return new Date(value).toISOString();
      }
    });
    
    let validationResults = validate(
      snapshot,
      {
        "DateTime": {
          "presence": true,
          "datetime": true
        },
        "Snapshots": {
          "presence": true,
          "type": "array"
        }
      }
    );
    
    if(validationResults) {
      return validationResults;
    }
    
    const result = await context.services.get("mongodb-atlas").db("game").collection("recordings").insertOne(snapshot);

    return result;
    
};