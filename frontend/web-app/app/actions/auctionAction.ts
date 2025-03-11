'use server'

import { auth } from "@/auth"
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
  const session = await auth();
  const response = await fetch('http://localhost:6001/auctions/0195835a-4c97-73a9-91c3-2cdd1f9062fd', {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${session?.accessToken}`
    },
    body: JSON.stringify(data)
  });
  if(!response.ok){
    return {status: response.status, message: response.statusText}
  }
  return response.statusText
}