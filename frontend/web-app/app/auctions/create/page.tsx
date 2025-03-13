import Heading from "@/app/components/Heading";
import AuctionForm from "../AuctionForm";

export default function Create() {
  return (
    <div className="mx-auto max-w-[75%] shadow-lg rounded-lg bg-white p-10">
      <Heading title="Sell your car!" subtitle="Please enter the details of your car"/>
      <AuctionForm/>
    </div>
  )
}