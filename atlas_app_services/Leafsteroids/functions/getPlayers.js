exports = async function({ query, headers, body}, response) {

    const { start_date, end_date } = query;

    
    let filter = {};
    
    if(start_date && end_date) {
      filter.CreatedAt = {
        "$gte": new Date(start_date),
        "$lte": new Date(end_date)
      }
    }
  
    // TO-DO: Use player_uniques collection 
    const docs = await context.services
      .get("mongodb-atlas")
      .db("Leafsteroids")
      .collection("players")
        .find(filter)
        .sort({Nickname:1})
        .toArray();

    return docs;
    
};
