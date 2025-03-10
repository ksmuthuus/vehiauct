'use client'
import { useState } from "react"
import { updateAuctionTest } from "../actions/auctionAction"
import { Button } from "flowbite-react"

export default function AuthTest() {
  const [loading, setLoading] = useState(false)
  const [data, setData] = useState<any>(undefined)

  function doUpdate(){
    setLoading(true)
    setData(undefined)
    updateAuctionTest()
      .then(res => setData(res))
      .catch(err => setData(err))
      .finally(() => setLoading(false))
  }
  return (
    <div className="flex items-center gap-4">
      <Button outline onClick={doUpdate} isProcessing={loading}>
        Test Auth
      </Button>
      <div>
        {JSON.stringify(data, null, 2)}
      </div>
    </div>
  )
}