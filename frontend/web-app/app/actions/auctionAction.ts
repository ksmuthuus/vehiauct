'use server'

import { Auction, PagedResult } from "@/types"

export async function getData(url: string): Promise<PagedResult<Auction>>{
  const response = await fetch(`http://localhost:6001/search${url}`)
  if(!response.ok){
    throw new Error('Failed to fetch data')
  }
  const data = await response.json()
  return data
}

export async function updateAuctionTest(){
  const data = {
    mileage: Math.floor(Math.random() * 10000) + 1,
  }
  const response = await fetch('http://localhost:6001/auctions', {
    method: 'PUT',
    headers: {
      
    },
    body: JSON.stringify(data)
  });
  if(!response.ok){
    return {status: response.status, message: response.statusText}
  }
  return response.json()
}