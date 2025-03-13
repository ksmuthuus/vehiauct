'use server'

import { Auction, PagedResult } from "@/types"
import { fetchWrapper } from "../lib/fetchWrapper"
import { FieldValues } from "react-hook-form"

export async function getData(url: string): Promise<PagedResult<Auction>>{
  return await fetchWrapper.get(`search${url}`)
}

export async function updateAuctionTest(){
  const data = {
    mileage: Math.floor(Math.random() * 10000) + 1,
  }

  return await fetchWrapper.put('auctions/0195835a-4c97-73a9-91c3-2cdd1f9062fd', data)
}

export async function createAuction(data:FieldValues){
  return await fetchWrapper.post('auctions', data)
}