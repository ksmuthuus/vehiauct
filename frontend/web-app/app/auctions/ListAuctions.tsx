
'use client'
import AuctionCard from "./AuctionCard"
import { Auction, PagedResult } from "@/types"
import AppPagination from "../components/AppPagination"
import { useEffect, useState } from "react"
import { getData } from "../actions/auctionAction"
import Filters from "./Filters"
import { useParamsStore } from "@/hooks/useParamsStore"
import { useShallow } from "zustand/shallow"
import qs from "query-string"
import EmptyFilter from "../components/EmptyFilter"


export default function List() {
// const [auctions, setAuctions] = useState<Auction[]>([])
// const [pageCount, setPageCount] = useState(0)
// const [pageNumber, setPageNumber] = useState(1)
// const [pageSize, setPageSize] = useState(4)

const[data, setData] = useState<PagedResult<Auction>>()
const params = useParamsStore(useShallow((state) => ({
  pageNumber: state.pageNumber,
  pageSize: state.pageSize,
  searchTerm: state.searchTerm,
  orderBy: state.orderBy,
  filterBy: state.filterBy,
  seller: state.seller,
  winner: state.winner
})))

const setParams = useParamsStore((state) => state.setParams)
const url = qs.stringifyUrl({url:'', query: params})
function setPageNumber(pageNumber: number){
  setParams({pageNumber}) 
}

useEffect(() => {
  getData(url).then((data) => {
    setData(data)
  })
},[url])

if(!data){
  return <h3>Loading...</h3>
}
return (
  <>
  <Filters />
  {data.totalCount === 0 ? (
    <EmptyFilter showReset/> 
  ): (
    <>
      <div className="grid grid-cols-4 gap-6">
      {data.results.map((auction) => (
        <AuctionCard key={auction.id} auction={auction}/>
      ))}
    </div>
    <div className="flex justify-center mt-4">
      <AppPagination 
        currentPage={params.pageNumber} 
        pageCount={data.pageCount} 
        pageChanged={setPageNumber}/>
      </div>
    </>
  )}
   
  </>
  
)
}