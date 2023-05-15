const validate = require("validate.js");

exports = async function({ query, headers, body}, response) {

    const { id } = query;
    
    let validationResults = validate.single(id, { "presence": { "message": "`id` cannot be blank" } });
    
    if(validationResults) {
      return validationResults;
    }
    
    let filter = {};
    
    filter._id = BSON.ObjectId(id);

    const docs = await context.services.get("mongodb-atlas").db("Leafsteroids").collection("recordings").findOne(filter);

    return  docs;
    
};
