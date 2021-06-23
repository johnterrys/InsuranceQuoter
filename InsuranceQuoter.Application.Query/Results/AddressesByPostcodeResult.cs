﻿namespace InsuranceQuoter.Application.Query.Results
{
    using System.Collections.Generic;
    using InsuranceQuoter.Message.Dtos;

    public class AddressesByPostcodeResult : QueryResult
    {
        public AddressesByPostcodeResult(IEnumerable<AddressDto> addresses)
        {
            Addresses = addresses;
        }

        public IEnumerable<AddressDto> Addresses { get; }
    }
}