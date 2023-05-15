exports = async function({ query, headers, body}, response) {

    const { start_date, end_date } = query;

    
    let filter = {};
    
    if(start_date && end_date) {
      filter.DateTime = {
        "$gte": new Date(start_date),
        "$lte": new Date(end_date)
      }
    }

    const docs = await context.services.get("mongodb-atlas").db("Leafsteroids").collection("recordings").find(filter).toArray();

    return  docs;
    
};
