# Introduction #

The NodeIterator is a class used to iterate through the tree of ExecutionNodes to collect all the TableEntity in the correct order.

Example of a tree.


Create 50 users with details and two accounts each. For each account, make a deposit then place 100 orders and transactions from that account. When all that is done, do a Withdraw from the account.
```
 /*
   * Node1 x50:
   * User
   * UserDetails
   * Account
   * Account
   *      Node2 x1:
   *      Deposit
   *          Node3 x100:
   *          Order
   *          Transaction
   *      Node3 x1
   *      Withdraw
         
   * 
   * Output:
   * [User, userDetails, acc, acc, [dep, [order, tran], [order, tran] ... [order, tran]], Withdraw]
   * 
   */
```