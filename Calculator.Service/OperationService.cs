using AutoMapper;
using AutoMapper.QueryableExtensions;
using Calculator.Model;
using Calculator.Model.Common.Enums;
using Calculator.Model.DataTransferObjects;
using Calculator.SQLServer.DAL.Repository;
using System.Security.Cryptography;

namespace Calculator.Service
{
    public class OperationService
    {
        private readonly IOperationRepository<OperationEntity> _operationRepository;
        private readonly IMapper _mapper;
        public OperationService(IOperationRepository<OperationEntity> operationRepository, IMapper mapper) {
            this._operationRepository = operationRepository;
            this._mapper = mapper;
            
        }

        /// <summary>
        /// Add/Update given operationDto to database
        /// </summary>
        /// <param name="operationDto"></param>
        /// <returns></returns>
        public OperationDto Add(OperationDto operationDto)
        {
            var operationUpdate = this._mapper.Map<OperationEntity>(operationDto);
            
            if(operationUpdate.MasterId <= 0)
            {
                this._operationRepository.InsertBulk(new List<OperationEntity> { operationUpdate });
            }
            else
            {
                //both MasterId and Id are greater than zero
                this._operationRepository.UpdateBulk(new List<OperationEntity> { operationUpdate});
            }

            var addedOperation = this._operationRepository.Get(x => 
            x.Inactive == false && x.ModifiedOn == null
            && x.MasterId == operationUpdate.MasterId
            ).FirstOrDefault();

            return this._mapper.Map<OperationDto>(addedOperation);
        }

        public OperationDto Remove(long masterId)
        {
          var removedMasterId = this._operationRepository.DeleteBulk(new List<long> { masterId })?.FirstOrDefault() ?? -1;
          var removedRecord = this._operationRepository.Get(x => x.Inactive && x.ModifiedOn == null && x.MasterId == removedMasterId)?.FirstOrDefault();
          return this._mapper.Map<OperationDto> (removedRecord);
        }

        public IEnumerable<OperationDto> GetOperationsHistory(long masterId)
        {
            if(this._operationRepository.Get(x => x.MasterId == masterId && !x.Inactive && x.ModifiedOn == null).Any())
            {
               return this._operationRepository
              .Get(x => !x.Inactive && x.MasterId == masterId)
              .AsEnumerable()
              .Select(x => this._mapper.Map<OperationDto>(x))
              .ToList();
            }
            else
            {
                return Enumerable.Empty<OperationDto>();    
            }
        }

        public IEnumerable<IGrouping<long, OperationDto>> GetOperationHistory()
        {
            var activeRecordsQuery = this._operationRepository
             .Get(x => !x.Inactive);

            var activeTipRecordQuery = this._operationRepository.Get(x => !x.Inactive && x.ModifiedOn == null);
            var activeRecordHisory = (from record in activeRecordsQuery
                                     join activeTipRecord in activeTipRecordQuery
                                     on record.MasterId equals activeTipRecord.MasterId
                                     select record).ToList();
           return activeRecordHisory
                .Select(x => this._mapper.Map<OperationDto>(x))
                .GroupBy(x => x.MasterId) ;
        }

        public void RemoveAll()
        {
            var toBeRemovedIds = this._operationRepository
                .Get(x => !x.Inactive && x.ModifiedOn == null)
                .Select(x => x.MasterId)
                .ToList();
            this._operationRepository.DeleteBulk(toBeRemovedIds);
        }

        public OperationDto FindLastActiveMemoryOperation(long masterId)
        {
            var op = this._operationRepository
                .Get(x => x.MasterId == masterId && !x.Inactive
                && (x.Type == OperationType.MemoryMinus || x.Type == OperationType.MemoryPlus))
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
            return this._mapper.Map<OperationDto>(op);
        }
        
    }
}
