// Copyright 2012-2013 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SQLDataProducer.RandomTests.Helpers;
using SQLDataProducer.Entities.ExecutionEntities;
//using System.Xml;
using SQLDataProducer.DataAccess.Factories;
using SQLDataProducer.Entities.Generators;
using System.Xml.Linq;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.Collections;
using SQLDataProducer.Entities.DatabaseEntities.Collections;

namespace SQLDataProducer.RandomTests.SerializationTests
{
    public class SaveLoadTests : TestBase
    {
        #region XmlDocumentStrings

        string columnsCollectionXML = @"<Columns>
        <Column ColumnName=""NewPersonId"">
          <Generator GeneratorName=""Identity From SQL Server"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""Name"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""BitColumn"">
          <Generator GeneratorName=""Random Bit"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""DecimalColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""BigintColumn"">
          <Generator GeneratorName=""Random Int"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""VarcharMaxColumn"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""SmallMoneyColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
      </Columns>";

        string fullDocumentXML = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ExecutionItemCollection>
  <ExecutionItem Description="""" Order=""1"" RepeatCount=""10"" TruncateBeforeExecution=""False"" UseIdentityInsert=""False"" ExecutionCondition=""None"" ExecutionConditionValue=""0"">
    <Table TableSchema=""Person"" TableName=""Address"">
      <Columns>
        <Column ColumnName=""AddressID"">
          <Generator GeneratorName=""Identity From SQL Server"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""AddressLine1"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278537988104"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""AddressLine2"">
          <Generator GeneratorName=""NULL value"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""City"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278537988104"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""StateProvinceID"">
          <Generator GeneratorName=""Random FOREIGN KEY Value (EAGER)"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Keys"" Value=""130002278538008105"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""PostalCode"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538008105"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""rowguid"">
          <Generator GeneratorName=""Random GUID"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""ModifiedDate"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
      </Columns>
    </Table>
  </ExecutionItem>
  <ExecutionItem Description="""" Order=""2"" RepeatCount=""1"" TruncateBeforeExecution=""False"" UseIdentityInsert=""False"" ExecutionCondition=""None"" ExecutionConditionValue=""0"">
    <Table TableSchema=""Person"" TableName=""NewPerson"">
      <Columns>
        <Column ColumnName=""NewPersonId"">
          <Generator GeneratorName=""Identity From SQL Server"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""Name"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538008105"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""BitColumn"">
          <Generator GeneratorName=""Random Bit"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""DecimalColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""BigintColumn"">
          <Generator GeneratorName=""Random Int"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""VarcharMaxColumn"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538008105"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""FloatColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""DateTime2Column"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""DateTimeColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""NCharFiveColumn"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538008105"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""DateColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""TimeColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538008105"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""SmallIntColumn"">
          <Generator GeneratorName=""Random Int"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""SmallDateTimeColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""SmallMoneyColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
      </Columns>
    </Table>
  </ExecutionItem>
  <ExecutionItem Description=""Peters exec item with none default values"" Order=""3"" RepeatCount=""66"" TruncateBeforeExecution=""True"" UseIdentityInsert=""True"" ExecutionCondition=""GreaterThan"" ExecutionConditionValue=""7"">
    <Table TableSchema=""Person"" TableName=""Address"">
      <Columns>
        <Column ColumnName=""AddressID"">
          <Generator GeneratorName=""Identity From SQL Server"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""AddressLine1"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""AddressLine2"">
          <Generator GeneratorName=""NULL value"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""City"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""StateProvinceID"">
          <Generator GeneratorName=""Random FOREIGN KEY Value (EAGER)"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Keys"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""PostalCode"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""rowguid"">
          <Generator GeneratorName=""Random GUID"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""ModifiedDate"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
      </Columns>
    </Table>
  </ExecutionItem>
  <ExecutionItem Description=""Peters exec item with none default values"" Order=""4"" RepeatCount=""66"" TruncateBeforeExecution=""True"" UseIdentityInsert=""True"" ExecutionCondition=""GreaterThan"" ExecutionConditionValue=""7"">
    <Table TableSchema=""Person"" TableName=""NewPerson"">
      <Columns>
        <Column ColumnName=""NewPersonId"">
          <Generator GeneratorName=""Identity From SQL Server"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""Name"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""BitColumn"">
          <Generator GeneratorName=""Random Bit"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""DecimalColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""BigintColumn"">
          <Generator GeneratorName=""Random Int"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""VarcharMaxColumn"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""FloatColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""DateTime2Column"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""DateTimeColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""NCharFiveColumn"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538048107"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""DateColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""TimeColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""SmallIntColumn"">
          <Generator GeneratorName=""Random Int"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""SmallDateTimeColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""SmallMoneyColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
      </Columns>
    </Table>
  </ExecutionItem>
</ExecutionItemCollection>";


        string columnXML = @" <Column ColumnName=""AddressLine1"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278537988104"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>";

        string parameterXML = @"<GeneratorParameter ParameterName=""Length"" Value=""130002278537988104"" IsWriteEnabled=""False"" />";

        

        string generatorXML = @"<Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278537988104"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>";

        string tableXML = @"<Table TableSchema=""Person"" TableName=""NewPerson"">
      <Columns>
        <Column ColumnName=""NewPersonId"">
          <Generator GeneratorName=""Identity From SQL Server"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""Name"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
      </Columns>
    </Table>";


        string executionItemXML = @"<ExecutionItem Description=""Peters exec item with none default values"" Order=""4"" RepeatCount=""66"" TruncateBeforeExecution=""True"" UseIdentityInsert=""True"" ExecutionCondition=""GreaterThan"" ExecutionConditionValue=""7"">
    <Table TableSchema=""Person"" TableName=""NewPerson"">
      <Columns>
        <Column ColumnName=""NewPersonId"">
          <Generator GeneratorName=""Identity From SQL Server"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""Name"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""BitColumn"">
          <Generator GeneratorName=""Random Bit"">
            <GeneratorParameters />
          </Generator>
        </Column>
        <Column ColumnName=""DecimalColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""BigintColumn"">
          <Generator GeneratorName=""Random Int"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""VarcharMaxColumn"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538028106"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""FloatColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""DateTime2Column"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538028106"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""DateTimeColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""NCharFiveColumn"">
          <Generator GeneratorName=""Countries"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Length"" Value=""130002278538048107"" IsWriteEnabled=""False"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""DateColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""TimeColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""SmallIntColumn"">
          <Generator GeneratorName=""Random Int"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""SmallDateTimeColumn"">
          <Generator GeneratorName=""Current Date"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
        <Column ColumnName=""SmallMoneyColumn"">
          <Generator GeneratorName=""Counting up Decimal"">
            <GeneratorParameters>
              <GeneratorParameter ParameterName=""MinValue"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""MaxValue"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Step"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
            </GeneratorParameters>
          </Generator>
        </Column>
      </Columns>
    </Table>
  </ExecutionItem>";

        #endregion


        public SaveLoadTests()
            :base()
        {
        }

        [Test]
        public void ShouldBeAbleToSaveAndLoadExecutionItemCollectionUsingManager()
        {
            var execItems = ExecutionItemHelper.GetRealExecutionItemCollection(Connection());
            ExecutionItemHelper.SetSomeParameters(execItems);
            var fileName = @"c:\temp\repeater\saved.xml";

            // Sanity checks
            {
                foreach (var item in execItems)
                {
                    Assert.IsTrue(item.Equals(item));
                    
                    // sanity check on the equals methods
                    foreach (var originCol in item.TargetTable.Columns)
                    {
                        var sameCol = (from cs in item.TargetTable.Columns
                                         where cs.ColumnName == originCol.ColumnName
                                         select cs).FirstOrDefault();
                        Assert.AreEqual(originCol.PossibleGenerators, sameCol.PossibleGenerators);
                    }
                }
            }

            var tda = new DataAccess.TableEntityDataAccess(Connection());
            ExecutionItemManager.Save(execItems, fileName);

            var loadedList = ExecutionItemManager.Load(fileName, tda);

            Assert.IsNotNull(loadedList);
            Assert.AreEqual(execItems.Count, loadedList.Count);
            Assert.AreEqual(execItems.IsContainingData, loadedList.IsContainingData);

            foreach (var item in execItems)
            {
                ExecutionItem loadedItem = loadedList.Where(x => x.TargetTable.Equals(item.TargetTable) && x.Order == item.Order).First();
                Assert.IsNotNull(loadedItem, string.Format("expected {0} to be loaded but was not found in loaded collection", item.TargetTable.ToString()));

                Assert.IsTrue(item.Description == loadedItem.Description);
                Assert.IsTrue(item.ExecutionCondition == loadedItem.ExecutionCondition);
                Assert.IsTrue(item.ExecutionConditionValue == loadedItem.ExecutionConditionValue);
                Assert.IsTrue(item.HasWarning == loadedItem.HasWarning);
                Assert.IsTrue(item.Order == loadedItem.Order);
                Assert.IsTrue(item.RepeatCount == loadedItem.RepeatCount);

                Assert.IsTrue(item.TruncateBeforeExecution == loadedItem.TruncateBeforeExecution);
                Assert.IsTrue(item.UseIdentityInsert == loadedItem.UseIdentityInsert);
                Assert.IsTrue(item.WarningText == loadedItem.WarningText);

                Assert.IsTrue(object.Equals(item.TargetTable, loadedItem.TargetTable));
                Assert.IsTrue(item.Equals(loadedItem));

                foreach (var originCol in item.TargetTable.Columns)
                {
                    var loadedCol = (from cs in loadedItem.TargetTable.Columns
                                     where cs.ColumnName == originCol.ColumnName
                                     select cs).FirstOrDefault();

                    Assert.IsNotNull(loadedCol);
                    PrintParameters(originCol);
                    PrintParameters(loadedCol);
                    Assert.AreEqual(originCol.PossibleGenerators, loadedCol.PossibleGenerators);
                }
            }
        }

        [Test]
        public void ShouldBeAbleToCreateColumnFromXMLDoc()
        {
            XElement columnElement = XElement.Parse(columnXML);

            ColumnEntity col = new ColumnEntity();
            col.ReadXml(columnElement);
            
            Assert.AreEqual("AddressLine1", col.ColumnName);
            Assert.AreEqual("Countries", col.Generator.GeneratorName);

            Assert.AreEqual(1, col.Generator.GeneratorParameters.Count);
            
            Console.WriteLine(columnElement);
        }

        [Test]
        public void ShouldBeAbleToCreateGeneratorFromXMLDoc()
        {
            XElement genElement = XElement.Parse(generatorXML);

            Generator generator = new Generator();
            generator.ReadXml(genElement);

            Assert.AreEqual("Countries", generator.GeneratorName);
            Assert.AreEqual(1, generator.GeneratorParameters.Count);

            Console.WriteLine(genElement);
        }

        [Test]
        public void ShouldBeAbleToCreateGeneratorParameterFromXMLDoc()
        {
            XElement paramElement = XElement.Parse(parameterXML);
            Console.WriteLine(paramElement);

            GeneratorParameter parr = new GeneratorParameter();
            parr.ReadXml(paramElement);

            //            <GeneratorParameter ParameterName="Length" Value="130002278537988104" IsWriteEnabled="False" />
            Assert.AreEqual("Length", parr.ParameterName);
            Assert.AreEqual("130002278537988104", parr.Value);
            Assert.AreEqual(false, parr.IsWriteEnabled);
        }


        string parameterCollectionXML = @"<GeneratorParameters>
              <GeneratorParameter ParameterName=""Shift Days"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Hours"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Minutes"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Seconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Shift Milliseconds"" Value=""130002278538048107"" IsWriteEnabled=""True"" />
              <GeneratorParameter ParameterName=""Randomness"" Value=""11"" IsWriteEnabled=""False"" />
            </GeneratorParameters>";

        string parameterCollectionXMLEMPTY = @"<GeneratorParameters />";

        [Test]
        public void ShouldBeAbleToCreateGeneratorParameterCollectionFromXMLDoc()
        {
            {
                XElement paramElement = XElement.Parse(parameterCollectionXML);
                Console.WriteLine(paramElement);

                GeneratorParameterCollection parras = new GeneratorParameterCollection();
                parras.ReadXml(paramElement);

                Assert.AreEqual(6, parras.Count);

                var p1 = parras[0];
                var p2 = parras[1];
                var p3 = parras[2];
                var p4 = parras[3];
                var p5 = parras[4];
                var p6 = parras[5];

                Assert.AreEqual("Shift Days", p1.ParameterName);
                Assert.AreEqual("130002278538048107", p1.Value);
                Assert.AreEqual(true, p1.IsWriteEnabled);

                Assert.AreEqual("Shift Hours", p2.ParameterName);
                Assert.AreEqual("130002278538048107", p2.Value);
                Assert.AreEqual(true, p2.IsWriteEnabled);

                Assert.AreEqual("Shift Minutes", p3.ParameterName);
                Assert.AreEqual("130002278538048107", p3.Value);
                Assert.AreEqual(true, p3.IsWriteEnabled);

                Assert.AreEqual("Shift Seconds", p4.ParameterName);
                Assert.AreEqual("130002278538048107", p4.Value);
                Assert.AreEqual(true, p4.IsWriteEnabled);

                Assert.AreEqual("Shift Milliseconds", p5.ParameterName);
                Assert.AreEqual("130002278538048107", p5.Value);
                Assert.AreEqual(true, p5.IsWriteEnabled);

                Assert.AreEqual("Randomness", p6.ParameterName);
                Assert.AreEqual("11", p6.Value);
                Assert.AreEqual(false, p6.IsWriteEnabled);
            }
            {
                XElement paramElement = XElement.Parse(parameterCollectionXMLEMPTY);
                Console.WriteLine(paramElement);

                GeneratorParameterCollection parras = new GeneratorParameterCollection();
                parras.ReadXml(paramElement);

                Assert.AreEqual(0, parras.Count);
            }

        }


        

        [Test]
        public void ShouldBeAbleToCreateColumnCollectionFromXMLDoc()
        {
            XElement columnCollectionElement = XElement.Parse(columnsCollectionXML);
            Console.WriteLine(columnCollectionElement);

            ColumnEntityCollection columns = new ColumnEntityCollection();
            columns.ReadXml(columnCollectionElement);

            Assert.AreEqual(7, columns.Count);
        }

        [Test]
        public void ShouldBeAbleToCreateExecutionItemFromXMLDoc()
        {
            XElement executionItemElement = XElement.Parse(executionItemXML);
            Console.WriteLine(executionItemElement);

            ExecutionItem ie = new ExecutionItem();
            ie.ReadXml(executionItemElement);

            Assert.IsNotNull(ie.TargetTable);
            Assert.AreEqual("NewPerson", ie.TargetTable.TableName);
            Assert.AreEqual("Peters exec item with none default values", ie.Description);
            Assert.AreEqual(66, ie.RepeatCount);
            Assert.AreEqual(true, ie.TruncateBeforeExecution);
            Assert.AreEqual(true, ie.UseIdentityInsert);
            Assert.AreEqual(Entities.ExecutionConditions.GreaterThan, ie.ExecutionCondition);
            Assert.AreEqual(7, ie.ExecutionConditionValue);
            Assert.AreEqual(4, ie.Order);
            Assert.AreEqual(15, ie.TargetTable.Columns.Count);
        }

        [Test]
        public void ShouldBeAbleToCreateExecutionItemCollectionFromXMLDoc()
        {
            XDocument exeColElement = XDocument.Parse(fullDocumentXML);
            Console.WriteLine(exeColElement);

            ExecutionItemCollection execItems = new ExecutionItemCollection();
            execItems.ReadXml(exeColElement);

            Assert.IsNotNull(execItems);
            Assert.AreEqual(4, execItems.Count);

            var e1 = execItems[0];
            var e2 = execItems[1];
            var e3 = execItems[2];
            var e4 = execItems[3];

            Assert.AreEqual("Address", e1.TargetTable.TableName);
            Assert.AreEqual("NewPerson", e2.TargetTable.TableName);
            Assert.AreEqual("Address", e3.TargetTable.TableName);
            Assert.AreEqual("NewPerson", e4.TargetTable.TableName);
        }

        [Test]
        public void ShouldBeAbleToCreateTableEntityFromXMLDoc()
        {
            XElement tableElement = XElement.Parse(tableXML);
            Console.WriteLine(tableElement);

            TableEntity table = new TableEntity();
            table.ReadXml(tableElement);

            Assert.AreEqual(2, table.Columns.Count);
            Assert.AreEqual("NewPerson", table.TableName);
            Assert.AreEqual("Person", table.TableSchema);
        }


        [Test]
        public void ShouldBeAbleToCreateExecutionItemCollectionUsingExecutionManager()
        {
            var tda = new DataAccess.TableEntityDataAccess(Connection());

            var loadedList = ExecutionItemManager.Load(XDocument.Parse(fullDocumentXML), tda);

            Assert.IsNotNull(loadedList);
            Assert.AreEqual(4, loadedList.Count);

            var e1 = loadedList[0];
            var e2 = loadedList[1];
            var e3 = loadedList[2];
            var e4 = loadedList[3];

            Assert.AreEqual("Address", e1.TargetTable.TableName);
            Assert.AreEqual("NewPerson", e2.TargetTable.TableName);
            Assert.AreEqual("Address", e3.TargetTable.TableName);
            Assert.AreEqual("NewPerson", e4.TargetTable.TableName);

            AssertLoadedExecutionItem(e1, "Person", "Address", true, "AddressID", "AddressLine1", "AddressLine2", "City", "StateProvinceID", "PostalCode", "rowguid", "ModifiedDate");
            AssertLoadedExecutionItem(e2, "Person", "NewPerson", true, "NewPersonId", "Name", "BitColumn", "DecimalColumn", "BigintColumn", "VarcharMaxColumn", "FloatColumn", "DateTime2Column"
                , "DateTimeColumn", "NCharFiveColumn", "DateColumn", "TimeColumn", "SmallIntColumn", "SmallDateTimeColumn", "SmallMoneyColumn");
            AssertLoadedExecutionItem(e3, "Person", "Address", true, "AddressID", "AddressLine1", "AddressLine2", "City", "StateProvinceID", "PostalCode", "rowguid", "ModifiedDate");
            AssertLoadedExecutionItem(e4, "Person", "NewPerson", true, "NewPersonId", "Name", "BitColumn", "DecimalColumn", "BigintColumn", "VarcharMaxColumn", "FloatColumn", "DateTime2Column"
                , "DateTimeColumn", "NCharFiveColumn", "DateColumn", "TimeColumn", "SmallIntColumn", "SmallDateTimeColumn", "SmallMoneyColumn");

            var e1ModifiedDateColGenerator = e1.TargetTable.Columns.Where(c => c.ColumnName == "ModifiedDate").FirstOrDefault().Generator;

            Assert.AreEqual("Current Date", e1ModifiedDateColGenerator.GeneratorName);
            Assert.AreEqual(5, e1ModifiedDateColGenerator.GeneratorParameters.Count);

            AssertParameters(e1ModifiedDateColGenerator,
                new KeyValuePair<string, string>("Shift Days", "130002278538008105"),
                new KeyValuePair<string, string>("Shift Hours", "130002278538008105"),
                new KeyValuePair<string, string>("Shift Minutes", "130002278538008105"),
                new KeyValuePair<string, string>("Shift Seconds", "130002278538008105"),
                new KeyValuePair<string, string>("Shift Milliseconds", "130002278538008105"));

        }

        private void AssertParameters(Generator generator, params KeyValuePair<string, string>[] keyPairs)
        {
            Assert.AreEqual(generator.GeneratorParameters.Count, keyPairs.Count());

            for (int i = 0; i < keyPairs.Count(); i++)
            {
                var parm = generator.GeneratorParameters[i];
                Assert.AreEqual(parm.ParameterName, keyPairs[i].Key);
                Assert.AreEqual(parm.Value, keyPairs[i].Value);
            }
        }

        private void AssertLoadedExecutionItem(ExecutionItem e1, string schemaName, string tableName, bool separator, params string[] colNames)
        {
            Assert.AreEqual(e1.TargetTable.TableSchema, schemaName);
            Assert.AreEqual(e1.TargetTable.TableName, tableName);
            for (int i = 0; i < colNames.Count(); i++)
            {
                Assert.AreEqual(e1.TargetTable.Columns[i].ColumnName, colNames[i]);
            }
        }

        private void PrintParameters(Entities.DatabaseEntities.ColumnEntity column)
        {
            foreach (var gen in column.PossibleGenerators)
            {
                foreach (var p in gen.GeneratorParameters)
                {
                    Console.WriteLine("{0} = {1}", p.ParameterName, p.Value);
                }
            }
            Console.WriteLine();
        }
    }
}
